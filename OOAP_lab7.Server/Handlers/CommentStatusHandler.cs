using OOAP_lab7.Server.Models;

namespace OOAP_lab7.Server.Handlers
{
    public class CommentStatusHandler : BaseModerationHandler
    {
        public override async Task<bool> HandleRequestAsync(ModerationRequest request)
        {
            bool canPerformAction = request.Action switch
            {
                ActionType.Approve => request.CommentToModerate.Status != CommentStatus.Approved,
                ActionType.Reject => request.CommentToModerate.Status != CommentStatus.Rejected,
                ActionType.MarkAsSpam => request.CommentToModerate.Status != CommentStatus.Spam,
                _ => true,
            };

            if (!canPerformAction)
            {
                Console.WriteLine($"Handler Failed: CommentStatus ({request.CommentToModerate.Status} cannot apply {request.Action})");
                return false;
            }
            Console.WriteLine($"Handler Passed: CommentStatus ({request.CommentToModerate.Status} can apply {request.Action})");
            return await PassToNextAsync(request);
        }
    }
}
