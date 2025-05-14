namespace Task.Strategy
{
    public interface INextTrackStrategy
    {
        string GetNext(List<string> playlist, string current);
    }
}
