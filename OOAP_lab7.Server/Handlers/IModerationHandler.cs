using OOAP_lab7.Server.Models;

namespace OOAP_lab7.Server.Handlers
{
    public interface IModerationHandler
    {
        IModerationHandler SetNext(IModerationHandler handler);
        Task<bool> HandleRequestAsync(ModerationRequest request);
    }
}
