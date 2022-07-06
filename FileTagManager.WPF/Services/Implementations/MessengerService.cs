using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Threading.Tasks;

namespace FileTagManager.WPF.Services
{
    public class MessengerService : IMessengerService
    {
        public void Send<TMessage>(TMessage message) where TMessage : class
        {
            WeakReferenceMessenger.Default.Send(message);
        }

        public void Register<TMessage>(Func<object, TMessage, Task> action) where TMessage : class
        {
            WeakReferenceMessenger.Default.Register<TMessage>(this, async (r, m) => await action(r, m));
        }
    }
}
