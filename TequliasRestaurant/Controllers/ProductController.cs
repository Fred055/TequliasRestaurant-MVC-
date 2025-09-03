using Microsoft.AspNetCore.Mvc;
using TequliasRestaurant.Data;
using TequliasRestaurant.Models;

namespace TequliasRestaurant.Controllers
{
    public class ProductController : Controller
    {
        private Repository<Product> products;
        private Repository<Ingredient> ingredients;
        private Repository<Category> categories;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.products = new Repository<Product>(context);
            ingredients = new Repository<Ingredient>(context);
            categories = new Repository<Category>(context);
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await products.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.Ingredients = await ingredients.GetAllAsync();
            ViewBag.Categories = await categories.GetAllAsync();
            if (id == 0)
            {
                ViewBag.Operation = "Add";
                return View(new Product());
            }
            else
            {
                Product product = await products.GetByIdAsync(id, new QueryOptions<Product>
                {
                    Includes = "ProductIngredients.Ingredient, Category",
                    Where = p => p.ProductId == id
                });
                ViewBag.operation = "Edit";
                return View(product);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(Product product, int[] ingredinetIds, int catId)
        {
            if (ModelState.IsValid)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(fileStream);
                }
                product.ImageUrl = uniqueFileName;
            }

            if (product.ProductId == 0)
            {
                ViewBag.Ingredients = await ingredients.GetAllAsync();
                ViewBag.Categories = await categories.GetAllAsync();
                product.CategoryId = catId;

                //add Ingredinets
                foreach (int id in ingredinetIds)
                {
                    product.ProductIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                }

                await products.AddAsync(product);
                return RedirectToAction("Index", "Product");
            }
            else
            {
                var existingProduct = await products.GetByIdAsync(product.ProductId, new QueryOptions<Product>
                {
                    Includes = "ProductIngredients"
                });
                if (existingProduct == null)
                {
                    ModelState.AddModelError("", "Product not found");
                    ViewBag.Ingredients = await ingredients.GetAllAsync();
                    ViewBag.Categories = await categories.GetAllAsync();
                    return View(product);
                }
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                existingProduct.CategoryId = catId;

                // Update ingredients
                existingProduct.ProductIngredients.Clear();
                foreach (int id in ingredinetIds)
                {
                    existingProduct.ProductIngredients.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                }

                try
                {
                    await products.UpdateAsync(existingProduct);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.GetBaseException().Message}");
                    ViewBag.Ingredients = await ingredients.GetAllAsync();
                    ViewBag.Categories = await categories.GetAllAsync();
                    return View(product);
                }


            }

            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await products.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Product not found.");
                return View("Index", await products.GetAllAsync());
            }
        }

    }
}
