using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// Configure session options
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
    // Uncomment the following lines if you want to add policies for Shipper and Employee
    // options.AddPolicy("Shipper", policy => policy.RequireRole("Shipper"));
    // options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
});

// Configure authentication with cookie scheme
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.Cookie.HttpOnly = true;
                  options.ExpireTimeSpan = TimeSpan.FromHours(3);
                  options.LoginPath = "/Home/Login";
                  options.LogoutPath = "/Home/Logout";
                  options.SlidingExpiration = true;
              });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add session middleware
app.UseSession();

// Add authentication middleware
app.UseAuthentication();

// Add authorization middleware
app.UseAuthorization();

// Configure endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "AdminHome",
        areaName: "Admin",
        pattern: "Admin/{controller=AdminHome}/{action=Index}/{id?}"
    );
    endpoints.MapAreaControllerRoute(
        name: "CustomerHome",
        areaName: "Customer",
        pattern: "Customer/{controller=CustomerHome}/{action=Index}/{id?}"
    );
    endpoints.MapAreaControllerRoute(
        name: "EmployeeHome",
        areaName: "Employee",
        pattern: "Employee/{controller=EmployeeHome}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
