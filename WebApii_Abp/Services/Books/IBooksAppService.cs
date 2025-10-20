using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using WebApii_Abp.Shared;

namespace WebApii_Abp.Books
{
    public partial interface IBooksAppService : IApplicationService
    {

        Task<PagedResultDto<BookDto>> GetListAsync(GetBooksInput input);

        Task<BookDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<BookDto> CreateAsync(BookCreateDto input);

        Task<BookDto> UpdateAsync(Guid id, BookUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(BookExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> bookIds);

        Task DeleteAllAsync(GetBooksInput input);
        Task<WebApii_Abp.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}