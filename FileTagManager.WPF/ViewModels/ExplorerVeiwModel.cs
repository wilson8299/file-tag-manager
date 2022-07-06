using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FileTagManager.WPF.Messages;
using System.Collections.ObjectModel;

namespace FileTagManager.WPF.ViewModels
{
    public class ExplorerVeiwModel : ObservableObject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public ObservableCollection<ExplorerVeiwModel> Children { get; set; } = new ObservableCollection<ExplorerVeiwModel>();
        
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set 
            {
                if (value) SelecteChanged();
                SetProperty(ref _isSelected, value);
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        private void SelecteChanged()
        {
            WeakReferenceMessenger.Default.Send(new ExplorerSelectedMessage(Path));
            WeakReferenceMessenger.Default.Send(new IsSelectedMessage(false));
        }
    }
}
