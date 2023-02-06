using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task CommitAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public void Commit()
        {
            //değişiklikleri veritabanına yansıt
            _appDbContext.SaveChanges();
        }
    }
}
