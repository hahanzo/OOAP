using OOAP_lab7.Server.Models;

namespace OOAP_lab7.Server.Handlers
{
    public class AuthenticationHandler : BaseModerationHandler
    {
        public override async Task<bool> HandleRequestAsync(ModerationRequest request)
        {
            if (request.Moderator == null || string.IsNullOrEmpty(request.Moderator.UserId))
            {
                Console.WriteLine("Handler Failed: Authentication");
                return false;
            }
            Console.WriteLine("Handler Passed: Authentication");
            return await PassToNextAsync(request);
        }
    }
}
