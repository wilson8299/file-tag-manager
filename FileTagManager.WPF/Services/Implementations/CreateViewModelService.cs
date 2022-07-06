using Autofac.Features.OwnedInstances;
using FileTagManager.WPF.ViewModels;
using System;

namespace FileTagManager.WPF.Services
{
    public class CreateViewModelService : ICreateViewModelService
    {
        private readonly Func<Owned<InitDirViewModel>> _initDirViewModel;
        private readonly Func<Owned<HomeViewModel>> _homeViewModel;
        private readonly Func<Owned<FileListViewModel>> _fileListViewModel;
        private readonly Func<Owned<SearchViewModel>> _searchViewModel;

        public CreateViewModelService(
            Func<Owned<InitDirViewModel>> initDirViewModel,
            Func<Owned<HomeViewModel>> homeViewModel,
            Func<Owned<FileListViewModel>> fileListViewModel,
            Func<Owned<SearchViewModel>> searchViewModel)
        {
            _initDirViewModel = initDirViewModel;
            _homeViewModel = homeViewModel;
            _fileListViewModel = fileListViewModel;
            _searchViewModel = searchViewModel;
        }

        public BaseViewModel CreateContainerViewModel(VCType vmType)
        {
            return vmType switch
            {
                VCType.InitDirView => _initDirViewModel().Value,
                VCType.HomeView => _homeViewModel().Value,
                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType"),
            };
        }

        public BaseViewModel CreateDetailViewModel(VDType vmType)
        {
            return vmType switch
            {
                VDType.FileListView => _fileListViewModel().Value,
                VDType.SearchView => _searchViewModel().Value,
                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType"),
            };
        }
    }
}
