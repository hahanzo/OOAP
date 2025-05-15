namespace Task.Observer
{
    public class StateConsoleLogger : IPlayerObserver
    {
        public void Update(string state, string currentTrack)
        {
            Console.WriteLine($"[Observer] Player changed to state and track: {state},{currentTrack}");
        }
    }
}
