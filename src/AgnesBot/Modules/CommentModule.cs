using AgnesBot.Core;
using AgnesBot.Domain;
using AgnesBot.Domain.Interfaces;
using Meebey.SmartIrc4net;

namespace AgnesBot.Modules
{
    public class CommentModule : BaseModule
    {
        private readonly ICommentRepository _commentRepository;

        public CommentModule(IIrcClient client, ICommentRepository commentRepository) : base(client)
        {
            _commentRepository = commentRepository;
        }

        public override void Process(IrcMessage message)
        {
            if(message.Type != ReceiveType.ChannelMessage)
                return;

            switch(message.MessageParts[0])
            {
                case "!add":
                    AddComment(message);
                    break;
            }
        }
        
        private void AddComment(IrcMessage message)
        {
            var text = message.Message.Replace("!add", string.Empty);

            if(string.IsNullOrEmpty(text))
                return;

            var comment = new Comment { Text = text };

            _commentRepository.SaveComment(comment);

            Client.SendMessage(SendType.Message, message.Channel, "Comment added");
        }
    }
}