using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FileTagManager.WPF.Messages
{
    public class ChangeWindowMessage : ValueChangedMessage<bool>
    {
        public ChangeWindowMessage(bool value) : base(value) { }
    }

    public class ChangeWindowMessage2 : ValueChangedMessage<bool>
    {
        public ChangeWindowMessage2(bool value) : base(value) { }
    }
}
