using GymManagement.Data;
using GymManagement.Data.Entities;
using GymManagement.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));

// Configures Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // User Configurations
    options.User.RequireUniqueEmail = true;

    // Password Configurations 
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<DataContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/NotAuthorized";
    options.AccessDeniedPath = "/Account/NotAuthorized";
});

builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IBlobHelper, BlobHelper>();
builder.Services.AddScoped<IConverterHelper, ConverterHelper>();

builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IGymRepository, GymRepository>();

builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

// Add Seed service
builder.Services.AddTransient<SeedDb>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Errors/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Aplica migrações e Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seed = services.GetRequiredService<SeedDb>();

    // Chama o método de seed
    await seed.InitializeAsync();
}

app.Run();
