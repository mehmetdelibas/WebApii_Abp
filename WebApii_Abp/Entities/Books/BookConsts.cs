namespace WebApii_Abp.Books
{
    public static class BookConsts
    {
        private const string DefaultSorting = "{0}CreationTime desc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Book." : string.Empty);
        }

    }
}