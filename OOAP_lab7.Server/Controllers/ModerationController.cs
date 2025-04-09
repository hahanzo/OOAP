namespace OOAP_lab7.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using OOAP_lab7.Server.Commands;
    using OOAP_lab7.Server.Data;
    using OOAP_lab7.Server.Handlers;
    using OOAP_lab7.Server.Models;
    using OOAP_lab7.Server.Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class ModerationController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly CommandManager _commandManager;
        private readonly IModerationHandler _moderationChain;

        public ModerationController(
            ICommentRepository commentRepository,
            CommandManager commandManager,
            IModerationHandler moderationChain)
        {
            _commentRepository = commentRepository;
            _commandManager = commandManager;
            _moderationChain = moderationChain;
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetPendingComments()
        {
            return Ok(await _commentRepository.GetPendingAsync());
        }

        [HttpPost("actions")]
        public async Task<IActionResult> PerformAction([FromBody] ModerationActionDto actionDto)
        {
            var comment = await _commentRepository.GetByIdAsync(actionDto.CommentId);
            if (comment == null)
            {
                return NotFound($"Comment with Id {actionDto.CommentId} not found.");
            }

            var moderator = new ModeratorInfo { UserId = "TestModerator", Role = ModeratorRole.Senior };

            var request = new ModerationRequest
            {
                Moderator = moderator,
                CommentToModerate = comment,
                Action = actionDto.Action
            };

            bool canExecute = await _moderationChain.HandleRequestAsync(request);

            if (!canExecute)
            {
                return Forbid("Moderator does not have permission or action is invalid for this comment.");
            }

            ICommand? command = actionDto.Action switch
            {
                ActionType.Approve => new ApproveCommentCommand(comment, _commentRepository),
                ActionType.Reject => new RejectCommentCommand(comment, _commentRepository),
                ActionType.MarkAsSpam => new MarkAsSpamCommand(comment, _commentRepository),
                _ => null
            };

            if (command == null)
            {
                return BadRequest("Invalid action type.");
            }

            try
            {
                await _commandManager.ExecuteCommandAsync(command);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error executing command: {ex}");
                return StatusCode(500, "An error occurred while executing the command.");
            }
        }

        [HttpPost("undo")]
        public async Task<IActionResult> UndoLastAction()
        {
            try
            {
                await _commandManager.UndoLastCommandAsync();
                return Ok("Last action undone successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("No action to undo.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error undoing command: {ex}");
                return StatusCode(500, "An error occurred while undoing the action.");
            }
        }
    }
}
