using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Web;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using WebApii_Abp.Books;
using WebApii_Abp.Permissions;
using WebApii_Abp.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;




namespace WebApii_Abp.Pages
{
    public partial class Books
    {
        
        
            
        
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        public DataGrid<BookDto> DataGridRef { get; set; }
        private IReadOnlyList<BookDto> BookList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateBook { get; set; }
        private bool CanEditBook { get; set; }
        private bool CanDeleteBook { get; set; }
        private BookCreateDto NewBook { get; set; }
        private Validations NewBookValidations { get; set; } = new();
        private BookUpdateDto EditingBook { get; set; }
        private Validations EditingBookValidations { get; set; } = new();
        private Guid EditingBookId { get; set; }
        private Modal CreateBookModal { get; set; } = new();
        private Modal EditBookModal { get; set; } = new();
        private GetBooksInput Filter { get; set; }
        private DataGridEntityActionsColumn<BookDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "book-create-tab";
        protected string SelectedEditTab = "book-edit-tab";
        private BookDto? SelectedBook;
        
        
        
        
        
        private List<BookDto> SelectedBooks { get; set; } = new();
        private bool AllBooksSelected { get; set; }
        
        public Books()
        {
            NewBook = new BookCreateDto();
            EditingBook = new BookUpdateDto();
            Filter = new GetBooksInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            BookList = new List<BookDto>();
            
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
                await SetBreadcrumbItemsAsync();
                await SetToolbarItemsAsync();
                await InvokeAsync(StateHasChanged);
            }
        }  

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Books"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewBook"], async () =>
            {
                await OpenCreateBookModalAsync();
            }, IconName.Add, requiredPolicyName: WebApii_AbpPermissions.Books.Create);

            return ValueTask.CompletedTask;
        }
        
        private void ToggleDetails(BookDto book)
        {
            DataGridRef.ToggleDetailRow(book, true);
        }
        
        private bool RowSelectableHandler( RowSelectableEventArgs<BookDto> rowSelectableEventArgs )
            => rowSelectableEventArgs.SelectReason is not DataGridSelectReason.RowClick && CanDeleteBook;
            
        private bool DetailRowTriggerHandler(DetailRowTriggerEventArgs<BookDto> detailRowTriggerEventArgs)
        {
            detailRowTriggerEventArgs.Toggleable = false;
            detailRowTriggerEventArgs.DetailRowTriggerType = DetailRowTriggerType.Manual;
            return true;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateBook = await AuthorizationService
                .IsGrantedAsync(WebApii_AbpPermissions.Books.Create);
            CanEditBook = await AuthorizationService
                            .IsGrantedAsync(WebApii_AbpPermissions.Books.Edit);
            CanDeleteBook = await AuthorizationService
                            .IsGrantedAsync(WebApii_AbpPermissions.Books.Delete);
                            
                            
        }

        private async Task GetBooksAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await BooksAppService.GetListAsync(Filter);
            BookList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetBooksAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await BooksAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("WebApii_Abp") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/books/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&BookIdMin={Filter.BookIdMin}&BookIdMax={Filter.BookIdMax}&BookName={HttpUtility.UrlEncode(Filter.BookName)}&PriceMin={Filter.PriceMin}&PriceMax={Filter.PriceMax}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<BookDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetBooksAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateBookModalAsync()
        {
            NewBook = new BookCreateDto{
                
                
            };

            SelectedCreateTab = "book-create-tab";
            
            
            await NewBookValidations.ClearAll();
            await CreateBookModal.Show();
        }

        private async Task CloseCreateBookModalAsync()
        {
            NewBook = new BookCreateDto{
                
                
            };
            await CreateBookModal.Hide();
        }

        private async Task OpenEditBookModalAsync(BookDto input)
        {
            SelectedEditTab = "book-edit-tab";
            
            
            var book = await BooksAppService.GetAsync(input.Id);
            
            EditingBookId = book.Id;
            EditingBook = ObjectMapper.Map<BookDto, BookUpdateDto>(book);
            
            await EditingBookValidations.ClearAll();
            await EditBookModal.Show();
        }

        private async Task DeleteBookAsync(BookDto input)
        {
            await BooksAppService.DeleteAsync(input.Id);
            await GetBooksAsync();
        }

        private async Task CreateBookAsync()
        {
            try
            {
                if (await NewBookValidations.ValidateAll() == false)
                {
                    return;
                }

                await BooksAppService.CreateAsync(NewBook);
                await GetBooksAsync();
                await CloseCreateBookModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditBookModalAsync()
        {
            await EditBookModal.Hide();
        }

        private async Task UpdateBookAsync()
        {
            try
            {
                if (await EditingBookValidations.ValidateAll() == false)
                {
                    return;
                }

                await BooksAppService.UpdateAsync(EditingBookId, EditingBook);
                await GetBooksAsync();
                await EditBookModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }









        protected virtual async Task OnBookIdMinChangedAsync(int? bookIdMin)
        {
            Filter.BookIdMin = bookIdMin;
            await SearchAsync();
        }
        protected virtual async Task OnBookIdMaxChangedAsync(int? bookIdMax)
        {
            Filter.BookIdMax = bookIdMax;
            await SearchAsync();
        }
        protected virtual async Task OnBookNameChangedAsync(string? bookName)
        {
            Filter.BookName = bookName;
            await SearchAsync();
        }
        protected virtual async Task OnPriceMinChangedAsync(double? priceMin)
        {
            Filter.PriceMin = priceMin;
            await SearchAsync();
        }
        protected virtual async Task OnPriceMaxChangedAsync(double? priceMax)
        {
            Filter.PriceMax = priceMax;
            await SearchAsync();
        }
        





        private Task SelectAllItems()
        {
            AllBooksSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllBooksSelected = false;
            SelectedBooks.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedBookRowsChanged()
        {
            if (SelectedBooks.Count != PageSize)
            {
                AllBooksSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedBooksAsync()
        {
            var message = AllBooksSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedBooks.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllBooksSelected)
            {
                await BooksAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await BooksAppService.DeleteByIdsAsync(SelectedBooks.Select(x => x.Id).ToList());
            }

            SelectedBooks.Clear();
            AllBooksSelected = false;

            await GetBooksAsync();
        }


    }
}
