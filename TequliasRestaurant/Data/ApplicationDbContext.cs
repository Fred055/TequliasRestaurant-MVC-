using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TequliasRestaurant.Models;

namespace TequliasRestaurant.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        // Define composite key and relationships for ProductIngredient in OnModelCreating
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure many-to-many relationship between Product and Ingredient via ProductIngredient
            modelBuilder.Entity<ProductIngredient>()
                .HasKey(pi => new { pi.ProductId, pi.IngredientId });
            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId);
            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);

            //Seed Data 
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Appetizers" },
                new Category { CategoryId = 2, Name = "Main Courses" },
                 new Category { CategoryId = 3, Name = "Side Dish" },
                new Category { CategoryId = 4, Name = "Desserts" },
                new Category { CategoryId = 5, Name = "Beverages" }
            );

            modelBuilder.Entity<Ingredient>().HasData(
                // mexican restauarant ingredients here
                new Ingredient { IngredientId = 1, Name = "Beef" },
                new Ingredient { IngredientId = 2, Name = "Chicken" },
                new Ingredient { IngredientId = 3, Name = "Fish" },
                new Ingredient { IngredientId = 4, Name = "Tortilla" },
                new Ingredient { IngredientId = 5, Name = "Lettuce" },
                new Ingredient { IngredientId = 6, Name = "Tomato" }

            );

            modelBuilder.Entity<Product>().HasData(
                // mexican restauarant food entries here
                new Product
                {
                    ProductId = 1,
                    Name = "Beef Tacos",
                    Description = "Delicious beef tacos with fresh toppings",
                    Price = 9.99m,
                    Stock = 100,
                    CategoryId = 2 // Main
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Chicken Taco",
                    Description = "A delicious chicken taco",
                    Price = 8.99m,
                    Stock = 150,
                    CategoryId = 2
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Fish Taco",
                    Description = "Delicious beef tacos with fresh toppings",
                    Price = 9.99m,
                    Stock = 102,
                    CategoryId = 2
                }
                );

            modelBuilder.Entity<ProductIngredient>().HasData(
                new ProductIngredient { ProductId = 1, IngredientId = 1 }, // Beef Tacos contains Beef
                new ProductIngredient { ProductId = 1, IngredientId = 4 }, // Beef Tacos contains Tortilla
                new ProductIngredient { ProductId = 1, IngredientId = 5 }, // Beef Tacos contains Lettuce
                new ProductIngredient { ProductId = 1, IngredientId = 6 }, // Beef Tacos contains Tomato
                new ProductIngredient { ProductId = 2, IngredientId = 2 }, // Chicken Taco contains Chicken
                new ProductIngredient { ProductId = 2, IngredientId = 4 }, // Chicken Taco contains Tortilla
                new ProductIngredient { ProductId = 2, IngredientId = 5 }, // Chicken Taco contains Lettuce
                new ProductIngredient { ProductId = 2, IngredientId = 6 }, // Chicken Taco contains Tomato
                new ProductIngredient { ProductId = 3, IngredientId = 3 }, // Fish Taco contains Fish
                new ProductIngredient { ProductId = 3, IngredientId = 4 }, // Fish Taco contains Tortilla
                new ProductIngredient { ProductId = 3, IngredientId = 5 }, // Fish Taco contains Lettuce
                new ProductIngredient { ProductId = 3, IngredientId = 6 }  // Fish Taco contains Tomato

                );
        }
    }
}

