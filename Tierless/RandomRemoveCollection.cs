using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Tierless;

public class RandomRemoveCollection<T>() : IList, IList<T>, ISet<T>
{
    private readonly IList<T> _list = new List<T>();
    private readonly IDictionary<T, int> _dictionary = new Dictionary<T, int>();

    public RandomRemoveCollection(IEnumerable<T> enumerable) : this()
    {
        int i = 0;
        foreach (var item in enumerable)
        {
            _list.Add(item);
            _dictionary.Add(item, i++);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void CopyTo(Array array, int index)
    {
        for (int i = index; i < _list.Count; i++)
        {
            array.SetValue(_list[i], i);
        }
    }

    public bool Remove(T item)
    {
        if (!_dictionary.TryGetValue(item, out int index))
            return false;
        
        _list[index] = _list[^1];
        _list.RemoveAt(_list.Count - 1);
        _dictionary.Remove(item);
        return true;
    }

    public int Count => _list.Count;

    public bool IsSynchronized => false;

    public object SyncRoot => throw new NotImplementedException();

    int IList.Add(object? value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "This collection does not support null values");

        return AddNotNull(value);
    }

    int AddNotNull(object value)
    {
        throw new NotImplementedException();
    }

    public void Add(T item)
    {
        _dictionary[item] = _dictionary.Count;
        _list.Add(item);
    }

    public void ExceptWith(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }

    public void IntersectWith(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }

    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }

    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }

    public bool IsSubsetOf(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }

    public bool IsSupersetOf(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }

    public bool Overlaps(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }

    public bool SetEquals(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }

    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }

    public void UnionWith(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }

    bool ISet<T>.Add(T item)
    {
        throw new NotImplementedException();
    }

    void ICollection<T>.Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(T item)
    {
        return _dictionary.ContainsKey(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        CopyTo((Array) array, arrayIndex);
    }

    void IList.Clear()
    {
        throw new NotImplementedException();
    }

    bool IList.Contains(object? value)
    {
        throw new NotImplementedException();
    }

    public int IndexOf(object value)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, object value)
    {
        throw new NotImplementedException();
    }

    void IList.Remove(object value)
    {
        throw new NotImplementedException();
    }

    public int IndexOf(T item)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, T item)
    {
        throw new NotImplementedException();
    }

    void IList<T>.RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    public T this[int index]
    {
        get => _list[index];
        set => throw new NotSupportedException("This collection does not support setting values in positions");
    }

    public int this[T key] => _dictionary[key];

    void IList.RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    public bool IsFixedSize => false;

    public bool IsReadOnly => false;

    object IList.this[int index]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public int RandomIndex()
    {
        return Random.RandomRangeInt(0, Count);
    }

    public T RandomItem()
    {
        return _list[RandomIndex()];
    }

    public void Shuffle()
    {
        _list.Shuffle();
        for (int i = 0; i < Count; i++)
        {
            _dictionary[_list[i]] = i;
        }
    }
}