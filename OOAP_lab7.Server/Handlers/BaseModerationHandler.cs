using OOAP_lab7.Server.Models;

namespace OOAP_lab7.Server.Handlers
{
    public abstract class BaseModerationHandler : IModerationHandler
    {
        protected IModerationHandler? _nextHandler;

        public IModerationHandler SetNext(IModerationHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public abstract Task<bool> HandleRequestAsync(ModerationRequest request);

        protected async Task<bool> PassToNextAsync(ModerationRequest request)
        {
            if (_nextHandler != null)
            {
                return await _nextHandler.HandleRequestAsync(request);
            }
            return true;
        }
    }
}
