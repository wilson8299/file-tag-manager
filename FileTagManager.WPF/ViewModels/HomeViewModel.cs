using FileTagManager.WPF.Services;
using System.Windows;
using System.Windows.Threading;

namespace FileTagManager.WPF.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private Dispatcher _dispatcher => Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

        public BaseViewModel SideBarViewModel { get; set; }
        public BaseViewModel FileDetailViewModel { get; set; }

        private INavigatorService _navigator;
        public INavigatorService Navigator
        {
            get => _navigator;
            set => SetProperty(ref _navigator, value);
        }

        public HomeViewModel(
            INavigatorService navigator,
            SideBarViewModel sideBarViewModel,
            FileDetailViewModel fileDetailViewModel)
        {
            Navigator = navigator;
            SideBarViewModel = sideBarViewModel;
            FileDetailViewModel = fileDetailViewModel;
        }
    }
}
