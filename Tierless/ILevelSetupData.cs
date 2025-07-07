using UnityEngine;

namespace Tierless;

public interface ILevelSetupData
{
    bool IsForLevel(int level);
    float TargetScore { get; }
    int MinEnemyTypeCount { get; }
    int MaxEnemyTypeCount { get; }
    
    public int MaxEnemyTypeCountLimited(int enemyTypeCount)
    {
        return Mathf.Min(MaxEnemyTypeCount, enemyTypeCount);
    }
}