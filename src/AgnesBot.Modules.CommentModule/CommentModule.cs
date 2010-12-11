using AgnesBot.Core.Irc;
using AgnesBot.Core.Modules;

namespace AgnesBot.Modules.CommentModule
{
    public class CommentModule : BaseModule
    {
        private readonly ICommentRepository _commentRepository;

        public CommentModule(IIrcClient client, ICommentRepository commentRepository) : base(client)
        {
            _commentRepository = commentRepository;

            AddHandler(data => data.Message.StartsWith("!comments add"), AddComment);
        }
        
        private void AddComment(IrcMessageData data)
        {
            Client.SendMessage(SendType.Message, data.Channel, "Comment added");
        }
    }
}