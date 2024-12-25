namespace DatPhongNhanh.Shared.Entities;

public abstract class EntityBase<T> : IEntityBase<T>
    where T : IEquatable<T>
{
    public T Id { get; set; }
}

public abstract class EntityAggregateBase<T> : HasDomainEventBase, IEntityBase<T>, IAggregateRoot
    where T : IEquatable<T>
{
    public T Id { get; set; }
}

public static class  EntityExtensions
{
    public static string? GetEntityIdName(this Type type)
    {
        if (type.IsSubclassOf(typeof(EntityBase<>)))
        {
            var idProperty = type.GetProperty("Id");
            if (idProperty != null)
            {
                var prefix = type.Name.Replace("Entity", "");
                return prefix + idProperty.Name;
            }
        }
        return null;
        
    }
}