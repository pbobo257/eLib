using eLib.Infrastructure;
using eLib.Infrastructure.Abstractions;
using eLib.Infrastructure.Repositories;
using eLib.Logic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace eLib
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>
        (options => options.UseSqlServer(
                    Configuration.GetConnectionString("default")));

            services.AddScoped<IAppService, AppService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IBookHeaderRepository, BookHeaderRepository>();
            services.AddScoped<IBookDetailsRepository, BookDetailsRepository>();

            services.AddTransient(typeof(LoginWindow));
            services.AddTransient(typeof(MainWindow));
        }
    }

   
}
