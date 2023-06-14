namespace DiagnosticsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create, build, and run a host that encapsulates all resources of the app, such as:
            //  - Dependency injection
            //  - Logging
            //  - Configuration 

            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Use Startup file to configure the application.
                    webBuilder.UseStartup<Startup>();
                });

    }
}