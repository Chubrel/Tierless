using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Tierless;

public class EnemyDataContext(IDictionary<EnemyData, CountAndScore> enemyDataCounter, int typeCount, float totalScore) : IEqualityComparer<EnemyDataContext>
{
    public static readonly EnemyDataContext Empty = new();

    public IDictionary<EnemyData, CountAndScore> EnemyDataCounter { get; } = enemyDataCounter;

    public int TypeCount { get; set; } = typeCount;

    public float TotalScore { get; set; } = totalScore;

    public EnemyDataContext() : this(new Dictionary<EnemyData, CountAndScore>(), 0, 0)
    {
    }

    public bool HasEnemy(EnemyData enemyData)
    {
        EnemyDataCounter.TryGetValue(enemyData, out var countAndScore);
        return countAndScore.Count > 0;
    }

    public CountAndScore CountAndScore(EnemyData enemyData)
    {
        EnemyDataCounter.TryGetValue(enemyData, out var countAndScore);
        return countAndScore;
    }

    public int Count(EnemyData enemyData)
    {
        return CountAndScore(enemyData).Count;
    }

    public float Score(EnemyData enemyData)
    {
        return CountAndScore(enemyData).Score;
    }

    public void JustAdd(EnemyData enemyData)
    {
        EnemyDataCounter.TryGetValue(enemyData, out var countAndScore);
        EnemyDataCounter[enemyData] = enemyData.Data.IncrementAndCountAndScore(countAndScore);
    }

    public void AddAndUpdate(EnemyData enemyData, int typeCount, float totalScore)
    {
        JustAdd(enemyData);
        UpdateCount();
        UpdateScore();
    }

    public void AddAndUpdate(EnemyData enemyData, int typeCount)
    {
        JustAdd(enemyData);
        TypeCount = typeCount;
        UpdateScore();
    }

    public void AddAndUpdate(EnemyData enemyData, float totalScore)
    {
        JustAdd(enemyData);
        UpdateCount();
        TotalScore = totalScore;
    }

    public void AddAndUpdate(EnemyData enemyData)
    {
        JustAdd(enemyData);
        UpdateCount();
        UpdateScore();
    }

    public void UpdateScore()
    {
        TotalScore = 0;
        foreach (var entry in EnemyDataCounter)
        {
            TotalScore += entry.Value.Score;
        }
    }

    public void UpdateCount()
    {
        TypeCount = 0;
        foreach (var entry in EnemyDataCounter)
        {
            if (entry.Value.Count > 0)
                TypeCount++;
        }
    }

    public EnemyDataContext Copy(int typeCount, float totalScore)
    {
        return new EnemyDataContext(EnemyDataCounter.ToDictionary(entry => entry.Key, entry => entry.Value), typeCount, totalScore);
    }
    
    public EnemyDataContext Copy(int typeCount)
    {
        return Copy(typeCount, TotalScore);
    }
    
    public EnemyDataContext Copy(float totalScore)
    {
        return Copy(TypeCount, totalScore);
    }
    
    public EnemyDataContext Copy()
    {
        return Copy(TypeCount, TotalScore);
    }
    
    public EnemyDataContext CopyAndAdd(EnemyData enemyData)
    {
        var copy = Copy();
        copy.AddAndUpdate(enemyData);
        return copy;
    }

    public IReadOnlyList<int> Counts()
    {
        return EnemyDataCounter.Aggregate(new List<int>(), (list, entries) =>
        {
            list.Add(entries.Value.Count);
            return list;
        });
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Counts());
    }

    public bool Equals(EnemyDataContext? x, EnemyDataContext? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        return x.GetType() == y.GetType() && x.Counts().Equals(y.Counts());
    }

    public int GetHashCode(EnemyDataContext obj)
    {
        return HashCode.Combine(obj.EnemyDataCounter, obj.TotalScore);
    }

    public IEnumerable<EnemyData> EnemyDataEnumerable()
    {
        return EnemyDataCounter.Select(entry => entry.Key);
    }
}
