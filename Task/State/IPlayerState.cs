using Task.Services;

namespace Task.State
{
    public interface IPlayerState
    {
        void Play(PlayerContext context);
        void Pause(PlayerContext context);
        void Stop(PlayerContext context);
        string Name {  get; }
    }
}
