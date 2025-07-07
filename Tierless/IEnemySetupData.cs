using System.Collections.Generic;

namespace Tierless;

public interface IEnemySetupData : IEqualityComparer<IEnemySetupData>
{
    public EnemyName Name { get; }

    public int Tier { get; }

    public float ScoreWithFormula(int count);

    public float ScoreWithFormula(CountAndScore countAndScore)
    {
        return ScoreWithFormula(countAndScore.Count);
    }

    public CountAndScore CountAndScoreWithFormula(int count)
    {
        return new CountAndScore(count, ScoreWithFormula(count));
    }

    public CountAndScore CountAndScoreWithFormula(CountAndScore countAndScore)
    {
        return CountAndScoreWithFormula(countAndScore.Count);
    }

    public CountAndScore IncrementAndCountAndScore(CountAndScore countAndScore)
    {
        return CountAndScoreWithFormula(countAndScore.Count + 1);
    }

    public CountAndScore DecrementAndCountAndScoreWithFormula(CountAndScore countAndScore)
    {
        return CountAndScoreWithFormula(countAndScore.Count - 1);
    }

    public int EvaluateMaxCount(float score);

    public int EvaluateMaxCount(CountAndScore countAndScore)
    {
        return EvaluateMaxCount(countAndScore.Score);
    }

    public CountAndScore EvaluateMaxCountAndScore(float score)
    {
        return CountAndScoreWithFormula(EvaluateMaxCount(score));
    }
    
    public CountAndScore EvaluateMaxCountAndScore(CountAndScore countAndScore)
    {
        return EvaluateMaxCountAndScore(countAndScore.Score);
    }

    bool IEqualityComparer<IEnemySetupData>.Equals(IEnemySetupData? x, IEnemySetupData? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        return x.GetType() == y.GetType() && x.Name.Equals(y.Name);
    }

    int IEqualityComparer<IEnemySetupData>.GetHashCode(IEnemySetupData obj)
    {
        return obj.Name.GetHashCode();
    }
}
