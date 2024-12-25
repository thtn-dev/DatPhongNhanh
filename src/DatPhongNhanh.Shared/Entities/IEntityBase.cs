namespace DatPhongNhanh.Shared.Entities;

public interface IEntityBase<T>
{
    T Id { get; set; }
}

public interface IDateTracking
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
}

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
}

