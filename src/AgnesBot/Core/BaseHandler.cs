using Meebey.SmartIrc4net;

namespace AgnesBot.Core
{
    public abstract class BaseHandler
    {
        protected readonly IrcClient Client;
        protected ReceiveType HandlesType { get; set; }
        protected string HandlesText { get; set; }

        protected BaseHandler(IrcClient client)
        {
            Client = client;
        }

        public abstract bool CanHandle(IrcMessageData data);
        public abstract void Handle(IrcMessageData data);
    }
}