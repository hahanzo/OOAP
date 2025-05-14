using Task.Observer;
using Task.State;
using Task.Strategy;

namespace Task.Services
{
    public class PlayerContext
    {
        private IPlayerState _state = new StoppedState();
        private readonly List<IPlayerObserver> _observers = new();
        private INextTrackStrategy _strategy = new SequentialStrategy();

        public List<string> Playlist { get; } = new List<string>();
        public string CurrentTrack { get; private set; } = "";

        public void AddTrack(string track)
        {
            if (!Playlist.Contains(track))
            {
                Playlist.Add(track);
                CurrentTrack = track;
            }
        }

        public void SetFirstTrack() 
        {
            CurrentTrack = Playlist[0];
        }

        public List<string> GetPlaylist () { return Playlist; }


        public string StateName => _state.Name;

        public void SetState(IPlayerState state)
        {
            _state = state;
            NotifyObservers();
        }

        public void Play() => _state.Play(this);
        public void Pause() => _state.Pause(this);
        public void Stop() => _state.Stop(this);

        public void Attach(IPlayerObserver observer) => _observers.Add(observer);

        private void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update(StateName, CurrentTrack);
            }
        }

        public string NextTrack()
        {
            CurrentTrack = _strategy.GetNext(Playlist, CurrentTrack);
            return CurrentTrack;
        }

        public void SetStrategy(INextTrackStrategy strategy) => _strategy = strategy;
    }
}
