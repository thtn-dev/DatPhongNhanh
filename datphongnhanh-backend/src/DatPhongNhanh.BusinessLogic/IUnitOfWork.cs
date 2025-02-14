using DatPhongNhanh.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatPhongNhanh.BusinessLogic
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
