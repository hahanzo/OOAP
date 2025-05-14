using Task.Services;

namespace Task.State
{
    public class PlayingState : IPlayerState
    {
        public void Play(PlayerContext context) {}

        public void Pause(PlayerContext context) => context.SetState(new PausedState());

        public void Stop(PlayerContext context) => context.SetState(new StoppedState());

        public string Name => "Playing";
    }
}
