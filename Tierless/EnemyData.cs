using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tierless;

public record EnemyData(GameObject GameObject, IEnemySetupData Data) : IEqualityComparer<EnemyData>
{
    public GameObject GameObject { get; } = GameObject;
    public IEnemySetupData Data { get; } = Data;

    public bool Equals(EnemyData? x, EnemyData? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        return x.GetType() == y.GetType() && x.Data.Equals(y.Data);
    }

    public int GetHashCode(EnemyData obj)
    {
        return obj.Data.GetHashCode();
    }
}
