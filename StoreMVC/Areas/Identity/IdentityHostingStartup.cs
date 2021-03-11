using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreModels;
using StoreDL;

[assembly: HostingStartup(typeof(StoreMVC.Areas.Identity.IdentityHostingStartup))]
namespace StoreMVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<CustomerDBContext>(options =>
                    options.UseNpgsql(
                        context.Configuration.GetConnectionString("StoreDB")));

                services.AddDefaultIdentity<StoreMVCUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<CustomerDBContext>();
            });

        }
    }
}