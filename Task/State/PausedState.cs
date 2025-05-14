using Task.Services;

namespace Task.State
{
    public class PausedState : IPlayerState
    {
        public void Play(PlayerContext context) => context.SetState(new PlayingState());

        public void Pause(PlayerContext context) {}

        public void Stop(PlayerContext context) => context.SetState(new StoppedState());

        public string Name => "Paused";
    }
}
