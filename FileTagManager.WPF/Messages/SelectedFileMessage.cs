using CommunityToolkit.Mvvm.Messaging.Messages;
using FileTagManager.Domain.Models;

namespace FileTagManager.WPF.Messages
{
    public class SelectedFileMessage : ValueChangedMessage<FileInfoModel>
    {
        public SelectedFileMessage(FileInfoModel value) : base(value) { }
    }
}
