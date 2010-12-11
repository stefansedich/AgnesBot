using AgnesBot.Core;
using AgnesBot.Core.Irc;
using AgnesBot.Core.Modules;
using AgnesBot.Modules.CommentModule.Domain;

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
            string comment = data.Message.Substring(14).Trim();

            if(string.IsNullOrEmpty(comment))
                return;

            _commentRepository.CreateComment(new Comment
                                               {
                                                   Text = comment,
                                                   Timestamp = SystemTime.Now()
                                               });

            Client.SendMessage(SendType.Message, data.Channel, "Comment has been added.");
        }
    }
}