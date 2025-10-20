using WebApii_Abp.Books;
using System;
using WebApii_Abp.Shared;
using WebApii_Abp.Books;
using Volo.Abp.AutoMapper;
using AutoMapper;

namespace WebApii_Abp.ObjectMapping;

public class WebApii_AbpAutoMapperProfile : Profile
{
    public WebApii_AbpAutoMapperProfile()
    {
        /* Create your AutoMapper object mappings here */

        CreateMap<Book, BookDto>();
        CreateMap<Book, BookExcelDto>();

        CreateMap<BookDto, BookUpdateDto>();
    }
}