using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebApii_Abp.Data;

public class WebApii_AbpDbContextFactory : IDesignTimeDbContextFactory<WebApii_AbpDbContext>
{
    public WebApii_AbpDbContext CreateDbContext(string[] args)
    {
        WebApii_AbpGlobalFeatureConfigurator.Configure();
        WebApii_AbpModuleExtensionConfigurator.Configure();
        
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<WebApii_AbpDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new WebApii_AbpDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}