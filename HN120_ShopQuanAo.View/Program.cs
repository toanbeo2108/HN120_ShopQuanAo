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
    options.AddPolicy("Admin", Role => Role.RequireRole("Admin"));
    options.AddPolicy("User", Role => Role.RequireRole("User"));
    // options.AddPolicy("Shipper", Role => Role.RequireRole("Shipper"));
    // options.AddPolicy("Employee", Role => Role.RequireRole("Employee"));
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication middleware
app.UseAuthentication();

// Add authorization middleware
app.UseAuthorization();

// Add session middleware
app.UseSession();

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