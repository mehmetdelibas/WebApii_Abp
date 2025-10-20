using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using WebApii_Abp.Data;

namespace WebApii_Abp.Books
{
    public abstract class EfCoreBookRepositoryBase : EfCoreRepository<WebApii_AbpDbContext, Book, Guid>
    {
        public EfCoreBookRepositoryBase(IDbContextProvider<WebApii_AbpDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        int? bookIdMin = null,
            int? bookIdMax = null,
            string? bookName = null,
            double? priceMin = null,
            double? priceMax = null,
            CancellationToken cancellationToken = default)
        {

            var query = await GetQueryableAsync();

            query = ApplyFilter(query, filterText, bookIdMin, bookIdMax, bookName, priceMin, priceMax);

            var ids = query.Select(x => x.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Book>> GetListAsync(
            string? filterText = null,
            int? bookIdMin = null,
            int? bookIdMax = null,
            string? bookName = null,
            double? priceMin = null,
            double? priceMax = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, bookIdMin, bookIdMax, bookName, priceMin, priceMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? BookConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            int? bookIdMin = null,
            int? bookIdMax = null,
            string? bookName = null,
            double? priceMin = null,
            double? priceMax = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, bookIdMin, bookIdMax, bookName, priceMin, priceMax);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Book> ApplyFilter(
            IQueryable<Book> query,
            string? filterText = null,
            int? bookIdMin = null,
            int? bookIdMax = null,
            string? bookName = null,
            double? priceMin = null,
            double? priceMax = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.BookName!.Contains(filterText!))
                    .WhereIf(bookIdMin.HasValue, e => e.BookId >= bookIdMin!.Value)
                    .WhereIf(bookIdMax.HasValue, e => e.BookId <= bookIdMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(bookName), e => e.BookName.Contains(bookName))
                    .WhereIf(priceMin.HasValue, e => e.Price >= priceMin!.Value)
                    .WhereIf(priceMax.HasValue, e => e.Price <= priceMax!.Value);
        }
    }
}