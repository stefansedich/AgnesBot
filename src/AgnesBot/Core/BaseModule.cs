using Meebey.SmartIrc4net;

namespace AgnesBot.Core
{
    public abstract class BaseModule
    {
        protected readonly IrcClient Client;
        protected ReceiveType HandlesType { get; set; }
        protected string HandlesText { get; set; }

        protected BaseModule(IrcClient client)
        {
            Client = client;
        }

        public abstract bool CanProcess(IrcMessageData data);
        public abstract void Process(IrcMessageData data);
    }
}