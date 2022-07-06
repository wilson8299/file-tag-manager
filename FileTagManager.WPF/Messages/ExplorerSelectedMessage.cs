using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FileTagManager.WPF.Messages
{
    public class ExplorerSelectedMessage : ValueChangedMessage<string>
    {
        public ExplorerSelectedMessage(string value) : base(value) { }
    }
}
