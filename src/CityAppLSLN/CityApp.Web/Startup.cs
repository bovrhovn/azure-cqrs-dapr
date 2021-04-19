using CityApp.Interfaces;
using CityApp.Services;
using CityApp.Web.Common;
using CityApp.Web.Hubs;
using CityApp.Web.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddScoped<ICategoryRepository, CategoryRepository>(
                _ => new CategoryRepository(connectionString));
            
            // adding local file implementation
            services.AddScoped<IFileWorker, LocalFileWorker>();

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((_) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddSignalR().AddAzureSignalR();

            services.AddRazorPages()
                .AddRazorPagesOptions(options => options.Conventions.AddPageRoute("/Info/Index", ""));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("CorsPolicy");
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