using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace TequliasRestaurant.Models
{
    public class Product
    {
        public Product()
        {

            ProductIngredients = new HashSet<ProductIngredient>();
        }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; } // Foreign key to Category, nullable if a product might not belong to any category

        [NotMapped]
        public IFormFile? ImageFile { get; set; } // Not mapped to the database, used for file uploads
        public string? ImageUrl { get; set; } = "https://via.placeholder.com/150";

        [ValidateNever]
        public Category? Category { get; set; }// A product can belong to one category

        [ValidateNever]
        public ICollection<OrderItem> OrderItems { get; set; }//A A product can be part of many order items

        [ValidateNever]
        public ICollection<ProductIngredient>? ProductIngredients { get; set; } // A product can have many ingredients
    }
}