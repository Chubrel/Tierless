using System.Collections.Generic;
using System.Linq;

namespace Tierless;

public class LevelSetupDataManager
{
    public const int StartLevelCount = 10;
    public const float StartTargetScore = 3.5f;
    public const float LastTargetScore = 20;
    
    private static LevelSetupDataManager? _instance;
    public static LevelSetupDataManager Instance => _instance ??= new LevelSetupDataManager();
    
    private readonly IList<ILevelSetupData> _data = new List<ILevelSetupData>();
    
    public void Add(ILevelSetupData data)
    {
        _data.Add(data);
    }

    public ILevelSetupData? Get(int level)
    {
        return _data.Reverse().FirstOrDefault(levelSetupData => levelSetupData.IsForLevel(level));
    }
    
    public ILevelSetupData GetOrDefault(int level)
    {
        return Get(level) ?? DefaultLevelSetupData.Default;
    }

    public void InitializeDefaults()
    {
        Add(new DefaultLevelSetupData(1, 1, 3.5f, 2, 2));
        Add(new DefaultLevelSetupData(2, 2, 5f, 2, 3));
        Add(new DefaultLevelSetupData(3, 3, 6.5f, 2, 3));
        Add(new DefaultLevelSetupData(4, 4, 8f, 2, 4));
        Add(new DefaultLevelSetupData(5, 5, 9.5f, 2, 5));
        Add(new DefaultLevelSetupData(6, 6, 11f, 3, 5));
        Add(new DefaultLevelSetupData(7, 7, 12.5f, 3, 6));
        Add(new DefaultLevelSetupData(8, 8, 14f, 3, 7));
        Add(new DefaultLevelSetupData(9, 9, 16f, 2, 7));
        Add(new DefaultLevelSetupData(10, 10, 18f, 2, 8));
        Add(new DefaultLevelSetupData(11, 0, 20f, 1, -1));
    }
}