using DatPhongNhanh.SharedKernel.Entities;

namespace DatPhongNhanh.BusinessLogic.User.Interfaces
{
    public interface ICurrentUser<T, TId>
        where T : EntityBase<TId>
        where TId : struct, IEquatable<TId>
    {
        T Get();
        Task<T> GetAsync();
        TId GetId();
        Task<TId> GetIdAsync();
    }
}
