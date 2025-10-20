using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace WebApii_Abp.Books
{
    public abstract class BookBase : FullAuditedAggregateRoot<Guid>
    {
        public virtual int BookId { get; set; }

        [CanBeNull]
        public virtual string? BookName { get; set; }

        public virtual double Price { get; set; }

        protected BookBase()
        {

        }

        public BookBase(Guid id, int bookId, double price, string? bookName = null)
        {

            Id = id;
            BookId = bookId;
            Price = price;
            BookName = bookName;
        }

    }
}