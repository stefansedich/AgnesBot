using AgnesBot.Core;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Core.UnitOfWork;
using AgnesBot.Modules.CommentModule.Domain;

namespace AgnesBot.Modules.CommentModule
{
    public class CommentModule : BaseModule
    {
        private readonly ICommentRepository _commentRepository;

        public CommentModule(IIrcClient client, ICommentRepository commentRepository) : base(client)
        {
            _commentRepository = commentRepository;

            AddHandler(
                new ModuleMessageHandler
                    {
                        Type = ReceiveType.ChannelMessage,
                        CommandRegex = "^!comments add",
                        Action = AddComment
                    }
                );

            AddHandler(
                new ModuleMessageHandler
                    {
                        Type = ReceiveType.ChannelMessage,
                        CommandRegex = "^!comments find",
                        Action = SearchComments
                    }
                );
        }
        
        private void AddComment(IrcMessageData data)
        {
            string text = data.MessageWithoutCommand;

            if (string.IsNullOrEmpty(text))
                return;

            UnitOfWork.Start(() => _commentRepository.CreateComment(new Comment
                                                                        {
                                                                            Text = text,
                                                                            Timestamp = SystemTime.Now()
                                                                        }));

            Client.SendMessage(SendType.Message, data.Channel, "Comment has been added.");
        }

        public void SearchComments(IrcMessageData data)
        {
            string text = data.MessageWithoutCommand;

            if (string.IsNullOrEmpty(text))
                return;

            UnitOfWork.Start(() =>
                                 {
                                     var comments = _commentRepository.SearchComments(text);
                                     
                                     foreach (var comment in comments)
                                         Client.SendMessage(SendType.Message, data.Channel, string.Format("{0} on {1}", comment.Text, comment.Timestamp));
                                 });
        }
    }
}