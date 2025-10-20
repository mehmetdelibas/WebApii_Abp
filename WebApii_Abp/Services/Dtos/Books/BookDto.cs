using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace WebApii_Abp.Books
{
    public abstract class BookDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public double Price { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}