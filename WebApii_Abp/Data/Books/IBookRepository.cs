using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebApii_Abp.Books
{
    public partial interface IBookRepository : IRepository<Book, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            int? bookIdMin = null,
            int? bookIdMax = null,
            string? bookName = null,
            double? priceMin = null,
            double? priceMax = null,
            CancellationToken cancellationToken = default);
        Task<List<Book>> GetListAsync(
                    string? filterText = null,
                    int? bookIdMin = null,
                    int? bookIdMax = null,
                    string? bookName = null,
                    double? priceMin = null,
                    double? priceMax = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            int? bookIdMin = null,
            int? bookIdMax = null,
            string? bookName = null,
            double? priceMin = null,
            double? priceMax = null,
            CancellationToken cancellationToken = default);
    }
}