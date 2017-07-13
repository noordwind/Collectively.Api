using System;
using Collectively.Api.Commands;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Messages.Commands.Remarks;

namespace Collectively.Api.Modules
{
    public class RemarkCommentModule : ModuleBase
    {
        public RemarkCommentModule(ICommandDispatcher commandDispatcher,
            IRemarkStorage remarkStorage,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, modulePath: "remarks/{remarkId}/comments")
        {
            Post("", async args => await For<AddCommentToRemark>()
                .SetResourceId(x => x.CommentId)
                .OnSuccessAccepted(x => $"remarks/{x.RemarkId}/comments/" + "{0}")
                .DispatchAsync());

            Put("{commentId}", async args => await For<EditRemarkComment>()
                .OnSuccessAccepted()
                .DispatchAsync());

            Delete("{commentId}", async args => await For<DeleteRemarkComment>()
                .OnSuccessAccepted()
                .DispatchAsync());

            Put("{commentId}/votes", async args => await For<SubmitRemarkCommentVote>()
                .Set(x => x.CreatedAt = DateTime.UtcNow)
                .OnSuccessAccepted()
                .DispatchAsync());

            Delete("{commentId}/votes", async args => await For<DeleteRemarkCommentVote>()
                .OnSuccessAccepted()
                .DispatchAsync());                
        }
    }
}