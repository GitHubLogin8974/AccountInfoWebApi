 
using AccountInfoWebApi.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AccountInfoWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfig = configuration;
        }

        public IConfiguration Configuration { get; }
        private static string PolicyName = "CORS-Policy";
        public static IConfiguration StaticConfig { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // DAL Dependency injection
            services.AddSingleton<IConnectionManager, DAL.SQL.ConnectionManager>();
            services.AddSingleton<IManageAccountInfo, DAL.DataManager.ManageAccountInfo>();

            // BLL Dependency injection
            services.AddSingleton<IAccountInfo, BLL.AccountInfo>();
            services.AddControllers();
            services.AddCors(option =>
            {
                option.AddPolicy(name: PolicyName, builder => {
                    builder.WithOrigins("http://localhost:3000/", "https://localhost:3000/")
                            .AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");

                // custom CSS
                c.InjectStylesheet("/swagger-ui/custom.css");
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
            app.UseCors(PolicyName);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
