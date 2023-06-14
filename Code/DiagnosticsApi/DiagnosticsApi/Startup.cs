using Microsoft.AspNetCore.Mvc;

namespace DiagnosticsApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime.
        // Use this method to add services to the container for Dependency Injection.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add the database with the connection string as set in appsettings.json
            //services.AddDbContext<ApiContext>(options =>
            //    options.UseSqlite(Configuration.GetConnectionString("SQLiteDatabase")));

            // Add Cross Origin Resource Sharing.
            services.AddCors(options =>
             options.AddPolicy("AllowAllHeaders",
                 builder =>
                 {
                     builder
                         .SetIsOriginAllowed(origin => true)
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials()
                         .SetPreflightMaxAge(TimeSpan.FromSeconds(600));
                 })
         );

            // Add the Controllers.
            services.AddControllersWithViews();

            services.AddLogging();

            // Add Swagger.
            services.AddSwaggerGen();

            // With this we don't have to specify [FromService] when using Dependency Injection.
            services.Configure<ApiBehaviorOptions>(options =>
                options.DisableImplicitFromServicesParameters = true);
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // If the app is in Development mode, we want to show the Exception page.
            // Otherwise redirect to the Error Controller that returns Internal Server Error.
            app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/error");

            app.UseCors("AllowAllHeaders");

            // Use Swagger for an overview of the API.
            app.UseSwagger();
            app.UseSwaggerUI();

            // Protection against man-in-the-middle attacks.
            app.UseHsts();

            // Add route matching.
            app.UseRouting();

            // Add endpoint execution.
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}