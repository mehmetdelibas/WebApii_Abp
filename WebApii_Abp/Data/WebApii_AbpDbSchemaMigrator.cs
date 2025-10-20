using Volo.Abp.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace WebApii_Abp.Data;

public class WebApii_AbpDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public WebApii_AbpDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        
        /* We intentionally resolving the WebApii_AbpDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<WebApii_AbpDbContext>()
            .Database
            .MigrateAsync();

    }
}
