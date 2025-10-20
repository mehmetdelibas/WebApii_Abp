using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApii_Abp.Books
{
    public abstract class BookCreateDtoBase
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public double Price { get; set; }
    }
}