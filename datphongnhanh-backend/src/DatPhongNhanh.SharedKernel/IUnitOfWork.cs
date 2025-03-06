
namespace DatPhongNhanh.SharedKernel;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
}
