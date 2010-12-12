using System.Collections.Generic;
using System.Text.RegularExpressions;
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
                        CommandRegex = new Regex("^!comments add (?<text>.+)$"),
                        Action = AddComment
                    }
                );

            AddHandler(
                new ModuleMessageHandler
                    {
                        Type = ReceiveType.ChannelMessage,
                        CommandRegex = new Regex("^!comments find (?<term>.+)$"),
                        Action = SearchComments
                    }
                );
        }
        
        private void AddComment(IrcMessageData data, IDictionary<string, string> commandData)
        {
            string text = commandData["text"];

            if (string.IsNullOrEmpty(text))
                return;

            UnitOfWork.Start(() => _commentRepository.CreateComment(new Comment
                                                                        {
                                                                            Text = text,
                                                                            Timestamp = SystemTime.Now()
                                                                        }));

            Client.SendMessage(SendType.Message, data.Channel, "Comment has been added.");
        }

        public void SearchComments(IrcMessageData data, IDictionary<string, string> commandData)
        {
            string text = commandData["term"];

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