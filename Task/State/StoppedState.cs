using Task.Services;

namespace Task.State
{
    public class StoppedState : IPlayerState
    {
        public void Play(PlayerContext context) => context.SetState(new PlayingState());

        public void Pause(PlayerContext context) {}

        public void Stop(PlayerContext context) {}

        public string Name => "Stopped";
    }
}
