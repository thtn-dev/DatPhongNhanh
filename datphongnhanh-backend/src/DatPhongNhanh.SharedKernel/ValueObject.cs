﻿namespace DatPhongNhanh.SharedKernel;
[Serializable]
public abstract class ValueObject : IEquatable<ValueObject>, IComparable, IComparable<ValueObject>
{
    private int? _cachedHashCode;
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override int GetHashCode()
    {
        _cachedHashCode ??= GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return current * 23 + (obj?.GetHashCode() ?? 0);
                }
            });

        return _cachedHashCode.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }
        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public virtual int CompareTo(object? obj)
    {
        if (obj == null)
            return 1;
        var thisType = GetUnproxiedType(this);
        var otherType = GetUnproxiedType(obj);
        if (thisType != otherType)
            return string.Compare(thisType.ToString(), otherType.ToString(), StringComparison.Ordinal);
        var other = (ValueObject)obj;
        var components = GetEqualityComponents().ToArray();
        var otherComponents = other.GetEqualityComponents().ToArray();
        return components.Select((t, i) => CompareComponents(t, otherComponents[i])).FirstOrDefault(comparison => comparison != 0);
    }

    public virtual int CompareTo(ValueObject? other)
    {
        return CompareTo(other as object);
    }

    private static int CompareComponents(object? object1, object? object2)
    {
        switch (object1)
        {
            case null when object2 is null:
                return 0;
            case null:
                return -1;
        }

        if (object2 is null)
            return 1;

        if (object1 is IComparable comparable1 && object2 is IComparable comparable2)
            return comparable1.CompareTo(comparable2);

        return object1.Equals(object2) ? 0 : -1;
    }

    public bool Equals(ValueObject? other)
    {
        return Equals(other);
    }

    public static bool operator ==(ValueObject a, ValueObject b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }

    internal static Type GetUnproxiedType(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        //const string efCoreProxyPrefix = "Castle.Proxies.";
        //const string nHibernateProxyPostfix = "Proxy";

        var type = obj.GetType();
        var typeString = type.ToString();

        //if (typeString.Contains(efCoreProxyPrefix) || typeString.EndsWith(nHibernateProxyPostfix))
        //    return type.BaseType!;

        return type;
    }
}