using System.Collections.Generic;
using UnityEngine;

namespace Tierless;

public class EnemySetupDataManager
{
    private static EnemySetupDataManager? _instance;
    public static EnemySetupDataManager Instance => _instance ??= new EnemySetupDataManager();
    
    private readonly Dictionary<EnemyName, IEnemySetupData> _dataByName = new();

    public void Add(IEnemySetupData enemySetupData)
    {
        _dataByName[enemySetupData.Name] = enemySetupData;
    }

    public IEnemySetupData? Get(EnemyName name)
    {
        _dataByName.TryGetValue(name, out var result);
        return result;
    }

    public IEnemySetupData? Get(string name)
    {
        return Get(EnemyName.FromAnyName(name));
    }

    public IEnemySetupData GetOrDefault(EnemyName name, int tier)
    {
        return Get(name) ?? GetDefault(name, tier);
    }
    
    public IEnemySetupData GetOrDefault(string name, int tier)
    {
        return Get(name) ?? GetDefault(name, tier);
    }
    
    public static IEnemySetupData GetDefault(EnemyName name, int tier)
    {
        return DefaultEnemySetupData.Default(name, tier);
    }
    
    public static IEnemySetupData GetDefault(string name, int tier)
    {
        return DefaultEnemySetupData.Default(name, tier);
    }

    public void InitializeDefaults()
    {
        Add(new DefaultEnemySetupData("Head", 3,
                new Vector2(1, 4), new Vector2(4, 20)
            ));
        Add(new DefaultEnemySetupData("Robe", 3,
                new Vector2(1, 4), new Vector2(4, 20)
            ));
        Add(new DefaultEnemySetupData("Runner", 3,
                new Vector2(1, 4), new Vector2(4, 20)
            ));
        Add(new DefaultEnemySetupData("Hunter", 3,
                new Vector2(1, 4), new Vector2(4, 20)
            ));
        Add(new DefaultEnemySetupData("Beamer", 3,
                new Vector2(1, 4), new Vector2(4, 20)
            ));
        Add(new DefaultEnemySetupData("Slow Walker", 3,
                new Vector2(1, 4), new Vector2(4, 20)
            ));
        Add(new DefaultEnemySetupData("Animal", 2,
                new Vector2(1, 2), new Vector2(3, 4), new Vector2(16, 20)
            ));
        Add(new DefaultEnemySetupData("Bowtie", 2,
                new Vector2(1, 2), new Vector2(3, 4), new Vector2(8, 20)
            ));
        Add(new DefaultEnemySetupData("Floater", 2,
                new Vector2(1, 2), new Vector2(3, 4), new Vector2(12, 20)
            ));
        Add(new DefaultEnemySetupData("Hidden", 2,
                new Vector2(1, 2), new Vector2(2, 4), new Vector2(8, 20)
            ));
        Add(new DefaultEnemySetupData("Tumbler", 2,
                new Vector2(1, 2), new Vector2(3, 4), new Vector2(16, 20)
            ));
        Add(new DefaultEnemySetupData("Upscream", 2,
                new Vector2(1, 2), new Vector2(3, 4), new Vector2(12, 20)
            ));
        Add(new DefaultEnemySetupData("Valuable Thrower", 2, 
                new Vector2(1, 2), new Vector2(3, 4), new Vector2(10, 20)
            ));
        Add(new DefaultEnemySetupData("Bang", 2,
                new Vector2(1, 2 / 3f), new Vector2(3, 2), new Vector2(6, 4), new Vector2(24, 20)
            ));
        Add(new DefaultEnemySetupData("Ceiling Eye", 1,
                new Vector2(1, 2 / 3f), new Vector2(3, 4), new Vector2(6, 11), new Vector2(10, 20)
            ));
        Add(new DefaultEnemySetupData("Duck", 1,
                new Vector2(1, 1), new Vector2(4, 4), new Vector2(12, 11), new Vector2(24, 20)
            ));
        Add(new DefaultEnemySetupData("Slow Mouth", 1,
                new Vector2(1, 1), new Vector2(4, 4), new Vector2(8, 11), new Vector2(12, 20)
            ));
        Add(new DefaultEnemySetupData("Thin Man", 1,
                new Vector2(1, 1), new Vector2(4, 4), new Vector2(8, 11), new Vector2(18, 20)
            ));
        Add(new DefaultEnemySetupData("Gnome", 1,
                new Vector2(1, 0.5f), new Vector2(4, 1), new Vector2(10, 4), new Vector2(20, 11), new Vector2(40, 20)
            ));
    }
}