using CommunityToolkit.Mvvm.ComponentModel;
using FileTagManager.WPF.ViewModels;

namespace FileTagManager.WPF.Services
{
    public class NavigatorService : ObservableObject, INavigatorService
    {
        private BaseViewModel _viewContainerNavigator;
        public BaseViewModel ViewContainerNavigator
        {
            get => _viewContainerNavigator;
            set => SetProperty(ref _viewContainerNavigator, value);
        }

        private BaseViewModel _viewDetailNavigator;
        public BaseViewModel ViewDetailNavigator
        {
            get => _viewDetailNavigator;
            set => SetProperty(ref _viewDetailNavigator, value);
        }
    }
}
