using System;
using UnityEngine;

namespace Tierless;

public record DefaultEnemySetupData(EnemyName Name, int Tier, params Vector2[] Parameters) : IEnemySetupData
{
    public DefaultEnemySetupData(string rawEnemyName, int tier, params Vector2[] parameters) : this(EnemyName.FromAnyName(rawEnemyName), tier, parameters)
    {
    }
    
    public EnemyName Name { get; } = Name;
    public int Tier { get; } = Tier;
    public Vector2[] Parameters { get; } = Parameters;

    public static float ScoreWithPoints(Vector2 point1, Vector2 point2, float x)
    {
        return Mathf.LerpUnclamped(point1.y, point2.y, Mathf.InverseLerp(point1.x, point2.x, x));
    }
    
    public float ScoreWithFormula(int count)
    {
        switch (Parameters.Length)
        {
            case 0:
                return 1;
            case 1:
                return Parameters[0].y;
        }

        Vector2 point1;
        var point2 = Parameters[0];
        int i = 1;
        do
        {
            point1 = point2;
            point2 = Parameters[i];
            if (count <= point2.x)
                break;
        } while (++i < Parameters.Length);

        return ScoreWithPoints(point1, point2, count);
    }

    public int EvaluateMaxCount(float score)
    {
        throw new NotImplementedException();
    }

    public static DefaultEnemySetupData Default(EnemyName enemyName, int tier)
    {
        float value = Mathf.Pow(2, tier - 1);
        return new DefaultEnemySetupData(enemyName, tier, new Vector2(1, value), new Vector2(2, 2 * value));
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
