using System;
using AgnesBot.Core.IrcUtils;

namespace AgnesBot.Core.Modules
{
    public interface IModule
    {
        void Process(IrcMessageData data);
    }
}