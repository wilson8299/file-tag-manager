using CommunityToolkit.Mvvm.Input;
using FileTagManager.WPF.Services;
using System;

namespace FileTagManager.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICreateViewModelService _createViewModelService;
        private readonly IConfigurationService _configurationService;

        private INavigatorService _navigator;
        public INavigatorService Navigator
        {
            get => _navigator;
            set => SetProperty(ref _navigator, value);
        }

        private IRelayCommand _viewLoadedCommand;
        public IRelayCommand ViewLoadedCommand =>
            _viewLoadedCommand ??= new RelayCommand(ViewLoaded);

        public MainViewModel(
            INavigatorService navigator,
            ICreateViewModelService createViewModelService,
            IConfigurationService configurationService)
        {
            Navigator = navigator;
            _createViewModelService = createViewModelService;
            _configurationService = configurationService;
        }

        private void ViewLoaded()
        {
            Navigator.ViewContainerNavigator = _configurationService.Get(bool.Parse, "isInit", false)
                ? _createViewModelService.CreateContainerViewModel(VCType.HomeView)
                : _createViewModelService.CreateContainerViewModel(VCType.InitDirView);
            Navigator.ViewDetailNavigator = _createViewModelService.CreateDetailViewModel(VDType.FileListView);
        }
    }
}
