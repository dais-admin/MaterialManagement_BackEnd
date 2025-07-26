namespace DAIS.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder CreateConfiguration(this WebApplicationBuilder builder)
        {
            var env = builder.Environment.EnvironmentName;
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
            return builder;
        }

    }
}
