using Microsoft.EntityFrameworkCore;
using WorthIt.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add SQLite database (this MUST be before builder.Build())
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=worthit.db")
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Expenses}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
