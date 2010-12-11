using System;
using AgnesBot.Core.Irc;

namespace AgnesBot.Core.Modules
{
    public interface IModule
    {
        void Process(IrcMessageData data);
        void AddHandler(Func<IrcMessageData, bool> predicate, Action<IrcMessageData> handler);
    }
}