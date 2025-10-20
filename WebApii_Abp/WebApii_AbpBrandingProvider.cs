using Microsoft.Extensions.Localization;
using WebApii_Abp.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace WebApii_Abp;

[Dependency(ReplaceServices = true)]
public class WebApii_AbpBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<WebApii_AbpResource> _localizer;

    public WebApii_AbpBrandingProvider(IStringLocalizer<WebApii_AbpResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
