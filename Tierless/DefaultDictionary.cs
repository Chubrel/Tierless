using System;
using System.Collections.Generic;

namespace Tierless;

public class DefaultDictionary<TK, TV>(Func<TV> defaultFactory) : Dictionary<TK, TV>
{
    public new TV this[TK key]
    {
        get
        {
            if (!TryGetValue(key, out TV result))
            {
                result = defaultFactory();
            }
            return result;
        }
        set => base[key] = value;
    }
}
