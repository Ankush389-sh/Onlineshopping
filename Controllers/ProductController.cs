using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnlineShopping.Models;
using System.Text.Json;

namespace OnlineShopping.Controllers
{
    // Controller responsible for product-related operations.
    public class ProductController : Controller
    {
        // Stores the memory cache object.
        private readonly IMemoryCache _memoryCache;

        // Stores the HTTP client used to call the external API.
        private readonly HttpClient _httpClient;

        // Constructor receives required services through dependency injection.
        public ProductController(
            IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory)
        {
            // Assign memory cache service.
            _memoryCache = memoryCache;

            // Create an HTTP client.
            _httpClient = httpClientFactory.CreateClient();
        }

        // Displays the product list.
        public async Task<IActionResult> Index()
        {
            // Store a sample customer name in a cookie.
            Response.Cookies.Append("CustomerName", "Ankush");

            // Store the selected product category in session.
            HttpContext.Session.SetString("SelectedCategory", "electronics");

            // Check whether product list already exists in memory cache.
            if (!_memoryCache.TryGetValue("ProductList", out List<Product>? products))
            {
                // Request product data asynchronously from Fake Store API.
                var response = await _httpClient.GetAsync(
                    "https://fakestoreapi.com/products");

                // Ensure that the API request was successful.
                response.EnsureSuccessStatusCode();

                // Read API response as text.
                var json = await response.Content.ReadAsStringAsync();

                // Convert JSON data into Product objects.
                products = JsonSerializer.Deserialize<List<Product>>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<Product>();

                // Store API result in memory cache for 10 minutes.
                _memoryCache.Set(
                    "ProductList",
                    products,
                    TimeSpan.FromMinutes(10));
            }

            // Send product data to the view.
            return View(products);
        }

        // Displays details of a selected product.
        [ResponseCache(
            Duration = 60,
            Location = ResponseCacheLocation.Any,
            NoStore = false)]
        public async Task<IActionResult> Details(int id)
        {
            // Retrieve product data asynchronously from the API.
            var response = await _httpClient.GetAsync(
                "https://fakestoreapi.com/products");

            // Ensure the API request was successful.
            response.EnsureSuccessStatusCode();

            // Read the API response.
            var json = await response.Content.ReadAsStringAsync();

            // Convert JSON into product objects.
            var products = JsonSerializer.Deserialize<List<Product>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Product>();

            // Find the product with the requested ProductId.
            var product = products.FirstOrDefault(p => p.ProductId == id);

            // Return a not-found response if product does not exist.
            if (product == null)
            {
                return NotFound();
            }

            // Send selected product to the details view.
            return View(product);
        }

        // Displays the order form.
        [HttpGet]
        public IActionResult Order(int id)
        {
            // Pass ProductId to the order view.
            ViewBag.ProductId = id;

            // Display the order form.
            return View();
        }

        // Processes the submitted order.
        [HttpPost]
        public IActionResult Order(int ProductId, string CustomerName)
        {
            // Store customer name in a cookie.
            Response.Cookies.Append("CustomerName", CustomerName);

            // Display confirmation message.
            ViewBag.Message =
                $"Order submitted successfully for Product ID: {ProductId}";

            // Return the order confirmation view.
            return View();
        }
    }
}
