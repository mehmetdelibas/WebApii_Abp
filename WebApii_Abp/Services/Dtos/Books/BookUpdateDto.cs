using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace WebApii_Abp.Books
{
    public abstract class BookUpdateDtoBase : IHasConcurrencyStamp
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public double Price { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}