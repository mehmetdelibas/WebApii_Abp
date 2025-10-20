using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace WebApii_Abp.Books
{
    public abstract class BookManagerBase : DomainService
    {
        protected IBookRepository _bookRepository;

        public BookManagerBase(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public virtual async Task<Book> CreateAsync(
        int bookId, double price, string? bookName = null)
        {

            var book = new Book(
             GuidGenerator.Create(),
             bookId, price, bookName
             );

            return await _bookRepository.InsertAsync(book);
        }

        public virtual async Task<Book> UpdateAsync(
            Guid id,
            int bookId, double price, string? bookName = null, [CanBeNull] string? concurrencyStamp = null
        )
        {

            var book = await _bookRepository.GetAsync(id);

            book.BookId = bookId;
            book.Price = price;
            book.BookName = bookName;

            book.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _bookRepository.UpdateAsync(book);
        }

    }
}