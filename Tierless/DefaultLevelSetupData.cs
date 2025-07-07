namespace Tierless;

public record DefaultLevelSetupData(int MinLevel, int MaxLevel, float TargetScore, int MinEnemyTypeCount, int MaxEnemyTypeCount) : ILevelSetupData
{
    public static readonly DefaultLevelSetupData Default = new(0, -1, 20, -1, -1);
    
    public int MinLevel { get; } = MinLevel;
    public int MaxLevel { get; } = MaxLevel;
    
    public bool IsForLevel(int level)
    {
        return level >= MinLevel && (MaxLevel < 0 || level <= MaxLevel);
    }

    public float TargetScore { get; } = TargetScore;
    public int MinEnemyTypeCount { get; } = MinEnemyTypeCount;
    public int MaxEnemyTypeCount { get; } = MaxEnemyTypeCount < 0 ? int.MaxValue : MaxEnemyTypeCount;
}
