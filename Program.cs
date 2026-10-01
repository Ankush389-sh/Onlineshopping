var builder = WebApplication.CreateBuilder(args);

// Add MVC controller and view support.
builder.Services.AddControllersWithViews();

// Add HttpClient for asynchronous API requests.
builder.Services.AddHttpClient();

// Add in-memory caching support.
builder.Services.AddMemoryCache();

// Add distributed memory cache required for session.
builder.Services.AddDistributedMemoryCache();

// Configure session management.
builder.Services.AddSession(options =>
{
    // Set the session timeout to 20 minutes.
    options.IdleTimeout = TimeSpan.FromMinutes(20);

    // Make the session cookie essential.
    options.Cookie.IsEssential = true;
});

// Add response caching support.
builder.Services.AddResponseCaching();

var app = builder.Build();

// Enable HTTPS redirection.
app.UseHttpsRedirection();

// Enable serving static files.
app.UseStaticFiles();

// Enable routing.
app.UseRouting();

// Enable response caching middleware.
app.UseResponseCaching();

// Enable session management.
app.UseSession();

// Enable authorization.
app.UseAuthorization();

// Configure the default MVC route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
