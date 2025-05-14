namespace Task.Observer
{
    public class ConsoleLogger : IPlayerObserver
    {
        public void Update(string state, string currentTrack)
        {
            Console.WriteLine($"[Observer] Player changed to state and track: {state},{currentTrack}");
        }
    }
}
