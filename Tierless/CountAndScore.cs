namespace Tierless;

public readonly struct CountAndScore(int count, float score)
{
    public int Count => count;
    
    public float Score => score;
    
    public CountAndScore RawIncremented => new(count + 1, score);
    
    public CountAndScore RawDecremented => new(count - 1, score);
}
