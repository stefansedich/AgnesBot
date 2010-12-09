using Meebey.SmartIrc4net;

namespace AgnesBot.Core
{
    public abstract class HandlerBase
    {
        protected ReceiveType HandlesType { get; set; }
        protected string HandlesText { get; set; }
        
        public bool CanHandle(IrcMessageData data)
        {
            if(string.IsNullOrEmpty(HandlesText))
                return false;

            return data.Type == HandlesType 
                && data.Message.StartsWith(HandlesText);
        }

        public abstract void Handle(IrcMessageData data, IrcClient client);

        protected internal HandlerBase Handles(ReceiveType receiveType)
        {
            HandlesType = receiveType;
            return this;
        }

        protected internal void WithText(string text)
        {
            HandlesText = text;
        }
    }
}