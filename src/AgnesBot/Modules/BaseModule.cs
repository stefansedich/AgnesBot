using AgnesBot.Core;

namespace AgnesBot.Modules
{
    public abstract class BaseModule
    {
        protected readonly IIrcClient Client;
        
        protected BaseModule(IIrcClient client)
        {
            Client = client;
        }
        
        public abstract void Process(IrcMessage message);
    }
}