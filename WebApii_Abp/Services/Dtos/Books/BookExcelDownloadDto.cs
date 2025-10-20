using Volo.Abp.Application.Dtos;
using System;

namespace WebApii_Abp.Books
{
    public abstract class BookExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public int? BookIdMin { get; set; }
        public int? BookIdMax { get; set; }
        public string? BookName { get; set; }
        public double? PriceMin { get; set; }
        public double? PriceMax { get; set; }

        public BookExcelDownloadDtoBase()
        {

        }
    }
}