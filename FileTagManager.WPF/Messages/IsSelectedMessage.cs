using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FileTagManager.WPF.Messages
{
    class IsSelectedMessage : ValueChangedMessage<bool>
    {
        public IsSelectedMessage(bool value) : base(value) { }
    }
}
