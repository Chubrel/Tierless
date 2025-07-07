namespace Tierless;

public record OptimizedEnemyDataContext(CountAndScore[] CountsAndScores, float TotalScore)
{
    public CountAndScore[] CountsAndScores { get; } = CountsAndScores;
    
    public float TotalScore { get; } = TotalScore;
}