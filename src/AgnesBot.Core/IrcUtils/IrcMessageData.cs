namespace AgnesBot.Core.IrcUtils
{
    public class IrcMessageData
    {
        public string Message { get; set; }
        public string MessageWithoutCommand { get; set; }
        public string Nickname { get; set; }
        public string From { get; set; }
        public string Channel { get; set; }
        public ReceiveType Type { get; set; }
    }
}