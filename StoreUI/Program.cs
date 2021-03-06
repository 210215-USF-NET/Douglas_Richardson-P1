using System;
using StoreBL;
using StoreDL;
using StoreDL.Entities;
using StoreDL.Mappers;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace StoreUI
{
    class Program
    {
        /// <summary>
        /// Douglas Richardson's Project 0
        /// The main method, setups the database connection
        /// </summary>
        /// <param name="args"></param>
        
        
        static void Main(string[] args)
        {

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            //db connection
            string connectionString = configuration.GetConnectionString("P0-Database");
            DbContextOptions<P0DatabaseContext> options = new DbContextOptionsBuilder<P0DatabaseContext>().EnableSensitiveDataLogging(true)
            .UseSqlServer(connectionString).Options;
            
            //new context
            using var context = new P0DatabaseContext(options);
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            //rollingInterval: RollingInterval.Day lets you log every day
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File(@"ourLog.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();


            //   __      _
            // o'')}____//
            // `_/      )
            // (_(_/-(_/

            Console.Clear();
            Console.WriteLine("Welcome to Tog Dog Pet Store! ");
            Console.WriteLine("How may we help you? ");
            //call method that starts main user interface
            UserBL userBL = new UserBL();
            MenuFactory menuFactory = new MenuFactory(userBL,context, new CartBL(new CartRepo(context, new CartMapper()), new OrderRepo(context, new OrderMapper()),userBL));
            menuFactory.Start();
        }
    }
}
