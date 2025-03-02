namespace DatPhongNhanh.OAuth.SharedKernel.Entities;

public abstract class EntityBase<T> : IEntityBase<T>
    where T : struct, IEquatable<T>
{
    public T Id { get; set; }
}