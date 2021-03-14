using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StoreDL;
using StoreModels;
using StoreBL;
using StoreMVC.Models.Mappers;
namespace StoreMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomerDBContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("StoreDB")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            /*            services.AddDefaultIdentity<StoreMVCUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<CustomerDBContext>();*/

            //Cookies
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".TopDog.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<ManagerStringBL>();
            services.AddScoped<ManagerStringRepo>();
            services.AddScoped<LocationBL>();
            services.AddScoped<LocationRepo>();
            services.AddScoped<LocationMapper>();
            services.AddScoped<ItemBL>();
            services.AddScoped<ItemRepo>();
            services.AddScoped<ItemMapper>();
            services.AddScoped<ProductBL>();
            services.AddScoped<ProductRepo>();
            services.AddScoped<CartBL>();
            services.AddScoped<CartRepo>();
            services.AddScoped<OrderBL>();
            services.AddScoped<OrderRepo>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Location}/{action=OneLocation}/{id?}");
                endpoints.MapRazorPages();
            });

            Task.Run(() => this.CreateRoles(roleManager)).Wait();
        }
        private async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Manager"))
            {
                await roleManager.CreateAsync(new IdentityRole("Manager"));
            }
        }


    }

}
