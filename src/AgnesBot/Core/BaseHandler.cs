using Meebey.SmartIrc4net;

namespace AgnesBot.Core
{
    public abstract class BaseHandler
    {
        protected ReceiveType HandlesType { get; set; }
        protected string HandlesText { get; set; }
        protected readonly IrcClient Client;

        protected BaseHandler(IrcClient client)
        {
            Client = client;
        }

        public abstract bool CanHandle(IrcMessageData data);
        public abstract void Handle(IrcMessageData data);
    }
}