using System;

namespace WebApii_Abp.Books;

public abstract class BookDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}