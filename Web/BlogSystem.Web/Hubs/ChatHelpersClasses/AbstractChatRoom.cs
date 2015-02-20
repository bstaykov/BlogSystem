namespace BlogSystem.Web.Hubs.ChatHelpersClasses
{
    public abstract class AbstractChatRoom
    {
        public abstract void Register(Participant participant);

        public abstract void Send(string from, string to, string message);
    }
}