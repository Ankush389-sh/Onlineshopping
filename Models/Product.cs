namespace OnlineShopping.Models
{
    // Represents a product in the online shopping system.
    public class Product
    {
        // Stores the unique ID of the product.
        public int ProductId { get; set; }

        // Stores the name of the product.
        public string ProductName { get; set; } = "";

        // Stores the category of the product.
        public string Category { get; set; } = "";

        // Stores the original price of the product.
        public decimal Price { get; set; }

        // Stores the discount percentage.
        public decimal DiscountPercentage { get; set; }
    }
}
