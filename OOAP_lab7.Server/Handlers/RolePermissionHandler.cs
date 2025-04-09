using OOAP_lab7.Server.Models;

namespace OOAP_lab7.Server.Handlers
{
    public class RolePermissionHandler : BaseModerationHandler
    {
        public override async Task<bool> HandleRequestAsync(ModerationRequest request)
        {
            bool canPerformAction = request.Action switch
            {
                ActionType.MarkAsSpam => request.Moderator.Role != ModeratorRole.Junior,
                _ => true,
            };

            if (!canPerformAction)
            {
                Console.WriteLine($"Handler Failed: RolePermission ({request.Moderator.Role} cannot {request.Action})");
                return false;
            }
            Console.WriteLine($"Handler Passed: RolePermission ({request.Moderator.Role} can {request.Action})");
            return await PassToNextAsync(request);
        }
    }
}
