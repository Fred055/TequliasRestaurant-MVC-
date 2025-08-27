namespace TequliasRestaurant.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; } // Foreign key to Category, nullable if a product might not belong to any category
        public Category? Category { get; set; }// A product can belong to one category
        public ICollection<OrderItem> OrderItems { get; set; }//A A product can be part of many order items
        public ICollection<ProductIngredient>? ProductIngredients { get; set; } // A product can have many ingredients
    }
}