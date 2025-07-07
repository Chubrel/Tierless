using System;
using UnityEngine;

namespace Tierless;

public record DefaultEnemySetupData(EnemyName Name, int Tier, params float[] Parameters) : IEnemySetupData
{
    public DefaultEnemySetupData(string rawEnemyName, int tier, params float[] parameters) : this(EnemyName.FromAnyName(rawEnemyName), tier, parameters)
    {
    }
    
    public EnemyName Name { get; } = Name;
    public int Tier { get; } = Tier;
    public float[] Parameters { get; } = Parameters;

    public float ScoreWithFormula(int count)
    {
        float result = 0;
        var x = 1;
        foreach (var t in Parameters)
        {
            result += t * x;
            x *= count - 1;
        }
        return result;
    }

    public int EvaluateMaxCount(float score)
    {
        throw new NotImplementedException();
        // return (int) Mathf.Floor(Mathf.Log(score / BaseScore, ScoreExponentBase)) + 1;
    }

    public static DefaultEnemySetupData Default(EnemyName enemyName, int tier)
    {
        return new DefaultEnemySetupData(enemyName, tier, Mathf.Pow(2, tier - 1), 1.5f);
    }

    public static DefaultEnemySetupData Default(string rawEnemyName, int tier)
    {
        return Default(EnemyName.FromAnyName(rawEnemyName), tier);
    }
    
    public Func<EnemyName, IEnemySetupData> GetDefaultFactory(int tier)
    {
        return enemyName => Default(enemyName, tier);
    }
}
