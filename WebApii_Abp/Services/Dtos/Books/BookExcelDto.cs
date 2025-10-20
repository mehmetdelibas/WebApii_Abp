using System;

namespace WebApii_Abp.Books
{
    public abstract class BookExcelDtoBase
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public double Price { get; set; }
    }
}