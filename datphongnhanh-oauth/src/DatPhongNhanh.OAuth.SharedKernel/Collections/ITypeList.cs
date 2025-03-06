using System.Collections;
using System.Reflection;

namespace DatPhongNhanh.OAuth.SharedKernel.Collections;

public interface ITypeList<in TBaseType> : IList<Type>, ICollection<Type>, IEnumerable<Type>, IEnumerable
{
    //
// Summary:
//     Adds a type to list.
//
// Type parameters:
//   T:
//     Type
    void Add<T>() where T : TBaseType;

//
// Summary:
//     Adds a type to list if it's not already in the list.
//
// Type parameters:
//   T:
//     Type
    bool TryAdd<T>() where T : TBaseType;

//
// Summary:
//     Checks if a type exists in the list.
//
// Type parameters:
//   T:
//     Type
    bool Contains<T>() where T : TBaseType;

//
// Summary:
//     Removes a type from list
//
// Type parameters:
//   T:
    void Remove<T>() where T : TBaseType;
}

public interface ITypeList : ITypeList<object>
{
}

public class TypeList<TBaseType> : ITypeList<TBaseType>, IList<Type>, ICollection<Type>, IEnumerable<Type>, IEnumerable
{
    private readonly List<Type> _typeList;

    //
    // Summary:
    //     Creates a new Volo.Abp.Collections.TypeList`1 object.
    public TypeList()
    {
        _typeList = new List<Type>();
    }

    //
    // Summary:
    //     Gets the count.
    //
    // Value:
    //     The count.
    public int Count => _typeList.Count;

    //
    // Summary:
    //     Gets a value indicating whether this instance is read only.
    //
    // Value:
    //     true if this instance is read only; otherwise, false.
    public bool IsReadOnly => false;

    //
    // Summary:
    //     Gets or sets the System.Type at the specified index.
    //
    // Parameters:
    //   index:
    //     Index.
    public Type this[int index]
    {
        get => _typeList[index];
        set
        {
            CheckType(value);
            _typeList[index] = value;
        }
    }

    public void Add<T>() where T : TBaseType
    {
        _typeList.Add(typeof(T));
    }

    public bool TryAdd<T>() where T : TBaseType
    {
        if (Contains<T>()) return false;

        Add<T>();
        return true;
    }

    public void Add(Type item)
    {
        CheckType(item);
        _typeList.Add(item);
    }

    public void Insert(int index, Type item)
    {
        CheckType(item);
        _typeList.Insert(index, item);
    }

    public int IndexOf(Type item)
    {
        return _typeList.IndexOf(item);
    }

    public bool Contains<T>() where T : TBaseType
    {
        return Contains(typeof(T));
    }

    public bool Contains(Type item)
    {
        return _typeList.Contains(item);
    }

    public void Remove<T>() where T : TBaseType
    {
        _typeList.Remove(typeof(T));
    }

    public bool Remove(Type item)
    {
        return _typeList.Remove(item);
    }

    public void RemoveAt(int index)
    {
        _typeList.RemoveAt(index);
    }

    public void Clear()
    {
        _typeList.Clear();
    }

    public void CopyTo(Type[] array, int arrayIndex)
    {
        _typeList.CopyTo(array, arrayIndex);
    }

    public IEnumerator<Type> GetEnumerator()
    {
        return _typeList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _typeList.GetEnumerator();
    }

    private static void CheckType(Type item)
    {
        if (!typeof(TBaseType).GetTypeInfo().IsAssignableFrom(item))
            throw new ArgumentException(
                $"Given type ({item.AssemblyQualifiedName}) should be instance of {typeof(TBaseType).AssemblyQualifiedName} ",
                "item");
    }
}

public class TypeList : TypeList<object>, ITypeList
{
}