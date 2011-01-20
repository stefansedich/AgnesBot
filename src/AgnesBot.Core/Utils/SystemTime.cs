using System;

namespace AgnesBot.Core.Utils
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}