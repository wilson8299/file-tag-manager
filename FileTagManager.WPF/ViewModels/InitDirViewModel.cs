using Autofac.Features.OwnedInstances;
using CommunityToolkit.Mvvm.Input;
using FileTagManager.Domain.Interfaces;
using FileTagManager.Domain.Models;
using FileTagManager.WPF.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FileTagManager.WPF.ViewModels
{
    public class InitDirViewModel : BaseViewModel
    {
        private readonly INavigatorService _navigatorService;
        private readonly ICreateViewModelService _createViewModelService;
        private readonly IOpenFileDialogService _openFileDialogService;
        private readonly IFileService _fileService;
        private readonly IConfigurationService _configurationService;
        private readonly IMapperService _mapperService;
        private readonly Func<Owned<IUnitOfWork>> _unitOfWork;
        private Dispatcher _dispatcher => Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

        private IRelayCommand _openFileDialogCommand;
        public IRelayCommand OpenFileDialogCommand =>
            _openFileDialogCommand ??= new RelayCommand(OpenFileDialog);

        private IAsyncRelayCommand _loadFilesAsyncCommand;
        public IAsyncRelayCommand LoadFilesAsyncCommand =>
            _loadFilesAsyncCommand ??= new AsyncRelayCommand(LoadFilesAsync);

        private string _dirPath;
        public string DirPath
        {
            get => _dirPath;
            set => SetProperty(ref _dirPath, value);
        }

        private LimitedSizeObservableCollection<string> _processFiles;
        public LimitedSizeObservableCollection<string> ProcessFiles
        {
            get => _processFiles;
            set => SetProperty(ref _processFiles, value);
        }

        public InitDirViewModel(
            ICreateViewModelService createViewModelService,
            INavigatorService navigatorService,
            IOpenFileDialogService openFileDialogService,
            IFileService fileService,
            IConfigurationService configurationService,
            IMapperService mapperService,
            Func<Owned<IUnitOfWork>> unitOfWork)
        {
            _navigatorService = navigatorService;
            _createViewModelService = createViewModelService;
            _openFileDialogService = openFileDialogService;
            _fileService = fileService;
            _configurationService = configurationService;
            _mapperService = mapperService;
            _unitOfWork = unitOfWork;

            ProcessFiles = new LimitedSizeObservableCollection<string>(60);
        }

        private void OpenFileDialog()
        {
            if (_openFileDialogService.SelectFolderDialog() != CommonFileDialogResult.Ok) return;
            DirPath = _openFileDialogService.SelectedPath;
        }

        private async Task LoadFilesAsync()
        {
            await Task.Run(async () =>
            {
                using var uow = _unitOfWork().Value;
                await uow.DirectoryInfoRepository.InsertAsync(new DirectoryInfoModel { Path = DirPath, Name = _fileService.SigleDirectoryInfo(DirPath)?.Name});
                foreach (var (fileInfo, isDirectory) in _fileService.GetInfo(DirPath))
                {
                    _dispatcher.Invoke(() => ProcessFiles.Add(fileInfo.Path));
                    if (isDirectory)
                    {
                        var directoryInfo = await _mapperService.MapSingleAsync<FileInfoModel, DirectoryInfoModel>(fileInfo);
                        await uow.DirectoryInfoRepository.InsertAsync(directoryInfo);
                    }
                    await uow.FileInfoRepository.InsertAsync(fileInfo);
                }
                uow.Commit();
            });

            _navigatorService.ViewContainerNavigator = _createViewModelService.CreateContainerViewModel(VCType.HomeView);
            _configurationService.Set("true", "isInit");
        }
    }
}
