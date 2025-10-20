using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebApii_Abp.Permissions;
using WebApii_Abp.Books;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using WebApii_Abp.Shared;

namespace WebApii_Abp.Books
{

    public abstract class BooksAppServiceBase : ApplicationService
    {
        protected IDistributedCache<BookDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IBookRepository _bookRepository;
        protected BookManager _bookManager;

        public BooksAppServiceBase(IBookRepository bookRepository, BookManager bookManager, IDistributedCache<BookDownloadTokenCacheItem, string> downloadTokenCache)
        {
            _downloadTokenCache = downloadTokenCache;
            _bookRepository = bookRepository;
            _bookManager = bookManager;

        }

        public virtual async Task<PagedResultDto<BookDto>> GetListAsync(GetBooksInput input)
        {
            var totalCount = await _bookRepository.GetCountAsync(input.FilterText, input.BookIdMin, input.BookIdMax, input.BookName, input.PriceMin, input.PriceMax);
            var items = await _bookRepository.GetListAsync(input.FilterText, input.BookIdMin, input.BookIdMax, input.BookName, input.PriceMin, input.PriceMax, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<BookDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Book>, List<BookDto>>(items)
            };
        }

        public virtual async Task<BookDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Book, BookDto>(await _bookRepository.GetAsync(id));
        }


        public virtual async Task DeleteAsync(Guid id)
        {
            await _bookRepository.DeleteAsync(id);
        }

 
        public virtual async Task<BookDto> CreateAsync(BookCreateDto input)
        {

            var book = await _bookManager.CreateAsync(
            input.BookId, input.Price, input.BookName
            );

            return ObjectMapper.Map<Book, BookDto>(book);
        }

 
        public virtual async Task<BookDto> UpdateAsync(Guid id, BookUpdateDto input)
        {

            var book = await _bookManager.UpdateAsync(
            id,
            input.BookId, input.Price, input.BookName, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Book, BookDto>(book);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(BookExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _bookRepository.GetListAsync(input.FilterText, input.BookIdMin, input.BookIdMax, input.BookName, input.PriceMin, input.PriceMax);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Book>, List<BookExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Books.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

  
        public virtual async Task DeleteByIdsAsync(List<Guid> bookIds)
        {
            await _bookRepository.DeleteManyAsync(bookIds);
        }


        public virtual async Task DeleteAllAsync(GetBooksInput input)
        {
            await _bookRepository.DeleteAllAsync(input.FilterText, input.BookIdMin, input.BookIdMax, input.BookName, input.PriceMin, input.PriceMax);
        }
        public virtual async Task<WebApii_Abp.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new BookDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new WebApii_Abp.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}