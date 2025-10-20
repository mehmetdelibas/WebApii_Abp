using WebApii_Abp.Localization;
using Volo.Abp.AspNetCore.Components;

namespace WebApii_Abp;

public abstract class WebApii_AbpComponentBase : AbpComponentBase
{
    protected WebApii_AbpComponentBase()
    {
        LocalizationResource = typeof(WebApii_AbpResource);
    }
}
