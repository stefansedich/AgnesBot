using Meebey.SmartIrc4net;

namespace AgnesBot.Core
{
    public abstract class HandlerBase
    {
        protected ReceiveType HandlesType { get; set; }
        protected string HandlesText { get; set; }

        public abstract bool CanHandle(IrcMessageData data);
        public abstract void Handle(IrcMessageData data, IrcClient client);
    }
}