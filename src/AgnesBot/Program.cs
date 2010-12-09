using System;
using AgnesBot.Core;

namespace AgnesBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new AgnesBotRunner().Start();

            Console.WriteLine("Press enter to exit...");
            Console.ReadKey();
        }
    }
}
