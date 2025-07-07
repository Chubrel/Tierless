using System.Collections.Generic;

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
        // Add(new DefaultEnemySetupData("Head", 3, 4.5f, 1.64f));
        // Add(new DefaultEnemySetupData("Robe", 3, 4.5f, 1.64f));
        // Add(new DefaultEnemySetupData("Runner", 3, 4, 1.5f));
        // Add(new DefaultEnemySetupData("Hunter", 3, 5, 1.59f));
        // Add(new DefaultEnemySetupData("Beamer", 3, 5, 1.59f));
        // Add(new DefaultEnemySetupData("Slow Walker", 3, 3.5f, 1.55f));
        // Add(new DefaultEnemySetupData("Animal", 2, 2f, 1.17f));
        // Add(new DefaultEnemySetupData("Bowtie", 2, 3f, 1.23f));
        // Add(new DefaultEnemySetupData("Floater", 2, 3f, 1.23f));
        // Add(new DefaultEnemySetupData("Hidden", 2, 3f, 1.23f));
        // Add(new DefaultEnemySetupData("Tumbler", 2, 2f, 1.15f));
        // Add(new DefaultEnemySetupData("Upscream", 2, 2.5f, 1.21f));
        // Add(new DefaultEnemySetupData("Valuable Thrower", 2, 3f, 1.23f));
        // Add(new DefaultEnemySetupData("Bang", 2, 0.67f, 1.16f));
        // Add(new DefaultEnemySetupData("Ceiling Eye", 1, 1.5f, 1.38f));
        // Add(new DefaultEnemySetupData("Duck", 1, 0.75f, 1.24f));
        // Add(new DefaultEnemySetupData("Slow Mouth", 1, 0.75f, 1.24f));
        // Add(new DefaultEnemySetupData("Thin Man", 1, 1.25f, 1.32f));
        // Add(new DefaultEnemySetupData("Gnome", 1, 0.3f, 1.11f));
        
        Add(new DefaultEnemySetupData("Head", 3, 20 /  4f, 20 / 5f));
        Add(new DefaultEnemySetupData("Robe", 3, 20 / 4f, 20 / 5f));
        Add(new DefaultEnemySetupData("Runner", 3, 20 / 4f, 20 / 5f));
        Add(new DefaultEnemySetupData("Hunter", 3, 20 / 4f, 20 / 5f));
        Add(new DefaultEnemySetupData("Beamer", 3, 20 / 4f, 20 / 5f));
        Add(new DefaultEnemySetupData("Slow Walker", 3, 20 / 4f, 20 / 5f));
        Add(new DefaultEnemySetupData("Animal", 2, 20 / 16f, 20 / 16f));
        Add(new DefaultEnemySetupData("Bowtie", 2, 20 / 8f, 20 / 8f));
        Add(new DefaultEnemySetupData("Floater", 2, 20 / 10f, 20 / 8f));
        Add(new DefaultEnemySetupData("Hidden", 2, 20 / 10f, 20 / 10f));
        Add(new DefaultEnemySetupData("Tumbler", 2, 20 / 12f, 20 / 12f));
        Add(new DefaultEnemySetupData("Upscream", 2, 20 / 12f, 20 / 12f));
        Add(new DefaultEnemySetupData("Valuable Thrower", 2, 20 / 10f, 20 / 10f));
        Add(new DefaultEnemySetupData("Bang", 2, 20 / 24f, 20 / 24f));
        Add(new DefaultEnemySetupData("Ceiling Eye", 1, 20 / 9f, 20 / 9f));
        Add(new DefaultEnemySetupData("Duck", 1, 20 / 16f, 20 / 16f));
        Add(new DefaultEnemySetupData("Slow Mouth", 1, 20 / 16f, 20 / 16f));
        Add(new DefaultEnemySetupData("Thin Man", 1, 20 / 11f, 20 / 11f));
        Add(new DefaultEnemySetupData("Gnome", 1, 20 / 40f, 20 / 40f));
    }
}