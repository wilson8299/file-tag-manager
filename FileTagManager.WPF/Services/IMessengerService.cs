using System;
using System.Threading.Tasks;

namespace FileTagManager.WPF.Services
{
    public interface IMessengerService
    {
        void Send<TMessage>(TMessage message) where TMessage : class;
        void Register<TMessage>(Func<object, TMessage, Task> action) where TMessage : class;
    }
}
