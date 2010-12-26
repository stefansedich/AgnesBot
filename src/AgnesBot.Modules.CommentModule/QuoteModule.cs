using System.Collections.Generic;
using System.Text.RegularExpressions;
using AgnesBot.Core;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Core.UnitOfWork;
using AgnesBot.Modules.QuoteModule.Domain;

namespace AgnesBot.Modules.QuoteModule
{
    public class QuoteModule : BaseModule
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuoteModule(IIrcClient client, IQuoteRepository quoteRepository) : base(client)
        {
            _quoteRepository = quoteRepository;

            AddHandler(
                new ModuleMessageHandler
                    {
                        Type = ReceiveType.ChannelMessage,
                        CommandRegex = new Regex("^!quotes add (?<text>.+)$"),
                        Action = AddQuote
                    }
                );

            AddHandler(
                new ModuleMessageHandler
                    {
                        Type = ReceiveType.ChannelMessage,
                        CommandRegex = new Regex("^!quotes find (?<term>.+)$"),
                        Action = SearchQuotes
                    }
                );
        }
        
        private void AddQuote(IrcMessageData data, IDictionary<string, string> commandData)
        {
            string text = commandData["text"];

            if (string.IsNullOrEmpty(text))
                return;

            UnitOfWork.Start(() => _quoteRepository.CreateQuote(new Quote
                                                                        {
                                                                            Text = text,
                                                                            Timestamp = SystemTime.Now()
                                                                        }));

            Client.SendMessage(SendType.Message, data.Channel, "Quote has been added.");
        }

        public void SearchQuotes(IrcMessageData data, IDictionary<string, string> commandData)
        {
            string text = commandData["term"];

            if (string.IsNullOrEmpty(text))
                return;

            UnitOfWork.Start(() =>
                                 {
                                     var comments = _quoteRepository.SearchQuotes(text, 3);
                                     
                                     foreach (var comment in comments)
                                         Client.SendMessage(SendType.Message, data.Channel, string.Format("{0} on {1}", comment.Text, comment.Timestamp));
                                 });
        }
    }
}