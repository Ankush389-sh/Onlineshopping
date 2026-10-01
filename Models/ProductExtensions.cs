namespace OnlineShopping.Models
{
    // Contains extension methods related to the Product class.
    public static class ProductExtensions
    {
        // Calculates and returns the selling price after discount.
        public static decimal GetDiscountedPrice(this Product product)
        {
            // Calculate the discount amount.
            decimal discountAmount =
                product.Price * product.DiscountPercentage / 100;

            // Subtract the discount from the original price.
            decimal discountedPrice =
                product.Price - discountAmount;

            // Return the final selling price.
            return discountedPrice;
        }
    }
}
