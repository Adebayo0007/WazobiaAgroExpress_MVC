using Agro_Express.ApplicationContext;
using Agro_Express.Email;
using Agro_Express.Repositories.Implementations;
using Agro_Express.Repositories.Interfaces;
using Agro_Express.Services.Implementations;
using Agro_Express.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
         builder.Configuration.GetConnectionString("AgroExpressConnectionString"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("AgroExpressConnectionString"))
         ));



        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        builder.Services.AddScoped<IAdminService, AdminService>();

        builder.Services.AddScoped<IFarmerRepository, FarmerRepository>();
        builder.Services.AddScoped<IFarmerService, FarmerService>();

        builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
        builder.Services.AddScoped<IBuyerService, BuyerService>();

        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductService, ProductService>();

        builder.Services.AddScoped<IRequestedProductRepository, RequestedProductRepository>();
        builder.Services.AddScoped<IRequestedProductService, RequestedProductService>();

        builder.Services.AddScoped<IEmailSender, EmailSender>();

        builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

        builder.Services.AddMvc(options =>
        {
            options.CacheProfiles.Add("Default",
            new CacheProfile{
                Duration = 3600
            });
        });//using cache for fast performace that will be implemented globally

        builder.Services.AddMemoryCache();  //using memory cache for storing data to avoiding hitting the db always

        

        


        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(config =>
       {
           config.LoginPath = "/Home/Index";
           config.LogoutPath = "/Home/Index";
           config.Cookie.Name = "WazobiaExpressApplication";
       });
        builder.Services.AddAuthorization();

        // //session

        builder.Services.AddSession(options =>
        {
            options.Cookie.Name = "WazobiaExpress.Session";
            options.IdleTimeout = TimeSpan.FromSeconds(50);
        });
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });

        }
     

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        //implementing versioning 
      /*  app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "versionedRoute",
                pattern: "v{version}/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });*/

        app.UseAuthentication(); 
        app.UseSession();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
        
    }
}