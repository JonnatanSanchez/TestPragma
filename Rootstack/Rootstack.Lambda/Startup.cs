using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GoogleScraping.Application;
using GoogleScraping.Application.Services;
using GoogleScraping.Domain.Repositories;
using GoogleScraping.Domain.ServiceAgent;
using GoogleScraping.Domain.Servicies;
using GoogleScraping.Infrastructure.Data.Repositories;
using System;
using System.IO;

namespace GoogleScraping.Lambda
{
    public class Startup
    {
        public IConfigurationRoot Configuration;
        public ServiceProvider serviceProvider;

        public Startup()
        {
            string enviroment = Environment.GetEnvironmentVariable("Enviroment");

            if (!string.IsNullOrEmpty(enviroment) && enviroment == "PDN")
            {
                Configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.Production.json", true, true).Build();
            }
            else
            {
                Configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.Development.json", true, true).Build();
            }
            AutoMapperConfig.Initialize();
            IServiceCollection services = new ServiceCollection();

            services.AddAutoMapper();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton(Mapper.Configuration);

            services.AddScoped<IMapper>(sp =>
  new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
            services.BuildServiceProvider();
            serviceProvider = services.BuildServiceProvider();

            //Application
            services.AddSingleton<ICategoriesApplicationService, CategoriesApplicationService>();
            services.AddSingleton<IProductsApplicationService, ProductsApplicationService>();
            services.AddSingleton<IFillDataBaseApplicationService, FillDataBaseApplicationService>();

            // Domain
            services.AddSingleton<IProductsDomainService, ProductsDomainService>();
            services.AddSingleton<ICategoriesDomainService, CategoriesDomainService>();
            services.AddSingleton<ICategoriesRepository, CategoriesRepository>();
            services.AddSingleton<IProductsRepository, ProductsRepository>();
            services.AddSingleton<IAppDatabaseObject, AppDatabaseObject>();
            services.AddSingleton<IFillDataBaseDomainService, FillDataBaseDomainService>();

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton(Mapper.Configuration);


            services.AddScoped<IMapper>(sp =>
              new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
            serviceProvider = services.BuildServiceProvider();

            //services.Add(new ServiceDescriptor(typeof(AppDatabaseObject), new AppDatabaseObject(Configuration["ConnectionStrings:Default"])));
            services.AddScoped<IAppDatabaseObject>(_ => new AppDatabaseObject());

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(AppDatabaseObject), new AppDatabaseObject()));
        }

    }
}
