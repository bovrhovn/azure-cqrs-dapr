using System.IO.Compression;
using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Services;
using CityApp.Web.Common;
using CityApp.Web.Factories;
using CityApp.Web.Hubs;
using CityApp.Web.Interfaces;
using CityApp.Web.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CityApp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<WebSettings>(Configuration);
            services.Configure<StorageOptions>(Configuration.GetSection("StorageOptions"));

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            //settings for connection string
            services.AddScoped<ICategoryRepository, CategoryRepository>(_ => new CategoryRepository(connectionString));
            services.AddScoped<ICityUserRepository, CityUserRepository>(_ => new CityUserRepository(connectionString));
            services.AddScoped<IElectricityRepository, ElectricityRepository>(_ => new ElectricityRepository(connectionString));
            services.AddScoped<IElectricityMeasurementRepository, ElectricityMeasurementRepository>(_ => new ElectricityMeasurementRepository(connectionString));
            services.AddScoped<INewsRepository, NewsRepository>(_ => new NewsRepository(connectionString));

            // adding local file implementation
            services.AddScoped<IFileWorker, LocalFileWorker>();
            services.AddSingleton<IUserDataContext, UserDataContext>();
            
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddScoped(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddHttpContextAccessor();

            services.AddHttpClient<INewsService, NewsService>();
            services.AddHttpClient<IElectricityService, ElectricityService>();
            services.AddScoped<IMessageService, MessageService>();
            
            services.AddControllers().AddDapr();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());
            services.Configure<GzipCompressionProviderOptions>(compressionOptions =>
                compressionOptions.Level = CompressionLevel.Optimal);
            
            services.AddSignalR().AddAzureSignalR();

            services.AddRazorPages()
                .AddRazorPagesOptions(options => options.Conventions.AddPageRoute("/Info/Index", ""));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            else app.UseExceptionHandler("/Error");

            app.UseStaticFiles();
            app.UseRouting();
            app.UseResponseCompression();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<AlertHub>("/alerts");
            });
        }
    }
}