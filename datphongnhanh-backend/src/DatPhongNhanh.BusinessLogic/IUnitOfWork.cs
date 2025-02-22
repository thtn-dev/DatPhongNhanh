using DatPhongNhanh.Data.DbContexts;
using DatPhongNhanh.SharedKernel;

namespace DatPhongNhanh.BusinessLogic
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellation = default)
        {
            return _dbContext.SaveChangesAsync(cancellation);
        }
    }
}
