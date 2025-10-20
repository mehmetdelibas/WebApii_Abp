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
    public class EfCoreBookRepository : EfCoreBookRepositoryBase, IBookRepository
    {
        public EfCoreBookRepository(IDbContextProvider<WebApii_AbpDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
          
        }
    }
}