using System.Collections.Generic;

namespace Tierless;

public class DereferencePreventer
{
    private static DereferencePreventer? _instance;
    public static DereferencePreventer Instance => _instance ??= new DereferencePreventer();
    
    // ReSharper disable once CollectionNeverQueried.Local
    private readonly Dictionary<string, HashSet<object>> _preventDereference = [];

    public bool IsGroupCreated(string group) => _preventDereference.ContainsKey(group);

    public bool DoesGroupHaveItems(string group)
    {
        _preventDereference.TryGetValue(group, out var collection);
        return collection?.Count > 0;
    }

    public void Add(string group, object endangered)
    {
        _preventDereference.TryGetValue(group, out var collection);
        if (collection is null)
        {
            collection = [];
            _preventDereference[group] = collection;
        }
        collection.Add(endangered);
    }

    public bool Remove(string group, object endangered)
    {
        _preventDereference.TryGetValue(group, out var collection);
        return collection is not null && collection.Remove(endangered);
    }
    
    public void Clear(string group)
    {
        _preventDereference.TryGetValue(group, out var collection);
        collection?.Clear();
    }
}