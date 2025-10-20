using Volo.Abp.Application.Services;
using WebApii_Abp.Localization;

namespace WebApii_Abp.Services;

/* Inherit your application services from this class. */
public abstract class WebApii_AbpAppService : ApplicationService
{
    protected WebApii_AbpAppService()
    {
        LocalizationResource = typeof(WebApii_AbpResource);
    }
}