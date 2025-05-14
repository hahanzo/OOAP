
namespace Task.Strategy
{
    public class SequentialStrategy : INextTrackStrategy
    {
        public string GetNext(List<string> playlist, string current)
        {
            int index = playlist.IndexOf(current);
            return playlist[(index + 1) % playlist.Count];
        }
    }
}
