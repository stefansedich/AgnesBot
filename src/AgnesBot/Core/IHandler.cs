using Meebey.SmartIrc4net;

namespace AgnesBot.Core
{
    public interface IHandler
    {
        bool CanHandle(IrcMessageData data);
        void Handle(IrcMessageData data, IrcClient client);
    }
}