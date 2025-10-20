using WebApii_Abp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace WebApii_Abp.Permissions;

public class WebApii_AbpPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WebApii_AbpPermissions.GroupName);

        myGroup.AddPermission(WebApii_AbpPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(WebApii_AbpPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(WebApii_AbpPermissions.MyPermission1, L("Permission:MyPermission1"));

        var bookPermission = myGroup.AddPermission(WebApii_AbpPermissions.Books.Default, L("Permission:Books"));
        bookPermission.AddChild(WebApii_AbpPermissions.Books.Create, L("Permission:Create"));
        bookPermission.AddChild(WebApii_AbpPermissions.Books.Edit, L("Permission:Edit"));
        bookPermission.AddChild(WebApii_AbpPermissions.Books.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WebApii_AbpResource>(name);
    }
}