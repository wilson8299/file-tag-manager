using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace FileTagManager.WPF.ViewModels
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : BaseViewModel;
    public class BaseViewModel : ObservableObject { }
    public class LimitedSizeObservableCollection<T> : ObservableCollection<T>
    {
        private int _capacity { get; }

        public LimitedSizeObservableCollection(int capacity)
        {
            _capacity = capacity;
        }

        public void Add(T item)
        {
            if (Count >= _capacity)
                RemoveAt(0);
            base.Add(item);
        }

        public void Add2First(T item)
        {
            if (Count >= _capacity)
                RemoveAt(_capacity - 1);
            base.Insert(0, item);
        }
    }
}
