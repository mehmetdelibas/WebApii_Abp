using WebApii_Abp.Books;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Gdpr;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;

namespace WebApii_Abp.Data;

public class WebApii_AbpDbContext : AbpDbContext<WebApii_AbpDbContext>
{
    public DbSet<Book> Books { get; set; } = null!;

    public const string DbTablePrefix = "App";
    public const string DbSchema = null;

    public WebApii_AbpDbContext(DbContextOptions<WebApii_AbpDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigurePermissionManagement();
        builder.ConfigureBlobStoring();
        builder.ConfigureIdentityPro();
        builder.ConfigureOpenIddictPro();
        builder.ConfigureGdpr();
        builder.ConfigureLanguageManagement();
        builder.ConfigureSaas();
        builder.ConfigureTextTemplateManagement();

        /* Configure your own entities here */
        if (builder.IsHostDatabase())
        {
            builder.Entity<Book>(b =>
            {
                b.ToTable(DbTablePrefix + "Books", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.BookId).HasColumnName(nameof(Book.BookId));
                b.Property(x => x.BookName).HasColumnName(nameof(Book.BookName));
                b.Property(x => x.Price).HasColumnName(nameof(Book.Price));
            });

        }
    }
}