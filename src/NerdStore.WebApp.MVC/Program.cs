using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NerdStore.Catalogo.Application.AutoMapper;
using NerdStore.Catalogo.Data;
using NerdStore.Pagamentos.Data;
using NerdStore.Vendas.Data;
using NerdStore.WebApp.MVC.Data;
using NerdStore.WebApp.MVC.Setup;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
services.AddDatabaseDeveloperPageExceptionFilter();

services.AddDbContext<CatalogoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddDbContext<VendasContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddDbContext<PagamentoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

services.AddControllersWithViews();

services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));

services.AddMediatR(typeof(Program));

services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();