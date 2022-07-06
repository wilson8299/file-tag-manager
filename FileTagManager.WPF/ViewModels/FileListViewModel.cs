using Autofac.Features.OwnedInstances;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileTagManager.Domain.Interfaces;
using FileTagManager.Domain.Models;
using FileTagManager.WPF.Helpers;
using FileTagManager.WPF.Messages;
using FileTagManager.WPF.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WinCopies.Util;

namespace FileTagManager.WPF.ViewModels
{
    public class FileListViewModel : BaseViewModel
    {
        private readonly IFileService _fileService;
        private readonly Func<Owned<IUnitOfWork>> _unitOfWork;

        private int _limit = 30;
        private int _page = 1;
        private int _total = 0;
        private string _parentPath = string.Empty;

        private bool _isBusying;
        public bool IsBusying
        {
            get => _isBusying;
            set => SetProperty(ref _isBusying, value);
        }

        private string _selectedDirectoryPath;
        public string SelectedDirectoryPath
        {
            get => _selectedDirectoryPath;
            set
            {
                SetProperty(ref _selectedDirectoryPath, value);
            }
        }

        private FileInfoModel _selectedFile;
        public FileInfoModel SelectedFile
        {
            get => _selectedFile;
            set
            {
                SelectedFileChanged(value);
                SetProperty(ref _selectedFile, value);
            }
        }

        private ObservableCollection<ExplorerVeiwModel> _directoryDto;
        public ObservableCollection<ExplorerVeiwModel> DirectoryDto 
        {
            get => _directoryDto;
            set => SetProperty(ref _directoryDto, value);
        }

        private ObservableCollection<FileInfoModel> _fileInfoDto;
        public ObservableCollection<FileInfoModel> FileInfoDto
        {
            get => _fileInfoDto;
            set => SetProperty(ref _fileInfoDto, value);
        }

        private IAsyncRelayCommand _viewLoadedAsyncCommand;
        public IAsyncRelayCommand ViewLoadedAsyncCommand =>
            _viewLoadedAsyncCommand ??= new AsyncRelayCommand(ViewLoadedAsync);

        private IRelayCommand _fileClickCommand;
        public IRelayCommand FileClickCommand =>
            _fileClickCommand ??= new RelayCommand(FileClick);

        private IAsyncRelayCommand _refreshExplorerAsyncCommand;
        public IAsyncRelayCommand RefreshExplorerAsyncCommand =>
            _refreshExplorerAsyncCommand ??= new AsyncRelayCommand(RefreshExplorerAsync);

        private IAsyncRelayCommand _refreshFileListAsyncCommand;
        public IAsyncRelayCommand RefreshFileListAsyncCommand =>
            _refreshFileListAsyncCommand ??= new AsyncRelayCommand(RefreshFileListAsync);
        
        private IAsyncRelayCommand _scrollToButtonCommand;
        public IAsyncRelayCommand ScrollToButtonCommand =>
            _scrollToButtonCommand ??= new AsyncRelayCommand(ScrollToButton);

        public FileListViewModel(
            IFileService fileService,
            Func<Owned<IUnitOfWork>> unitOfWork)
        {
            _fileService = fileService;
            _unitOfWork = unitOfWork;

            WeakReferenceMessenger.Default.Register<ExplorerSelectedMessage>(this, async (r, m) => await LoadFilesAsync(r, m));
        }

        private async Task ViewLoadedAsync()
        {
            IsBusying = true;
            using var uow = _unitOfWork().Value;
            var directories = await uow.DirectoryInfoRepository.GetAllAsync();
            uow.Commit();

            DirectoryDto = await _fileService.GetDirectoriesAsync(directories, 0);
            DirectoryDto.FirstOrDefault().IsSelected = true;
            IsBusying = false;
        }

        private async Task LoadFilesAsync(object r, ExplorerSelectedMessage m)
        {
            IsBusying = true;
            _page = 1;
            SelectedDirectoryPath = m.Value;

            using var uow = _unitOfWork().Value;
            _total = await uow.FileInfoRepository.GetRowCount(SelectedDirectoryPath);
            var filesInfo = await uow.FileInfoRepository.GetByPaginationAsync(SelectedDirectoryPath, _limit, _limit * (_page - 1));
            uow.Commit();

            FileInfoDto = await _fileService.GetFileThumbnailAsync(filesInfo);
            IsBusying = false;
        }

        private async Task RefreshExplorerAsync()
        {
            IsBusying = true;
            var newDirectories = await _fileService.GetDirectoriesAsync(DirectoryDto.First().Path);
            if (newDirectories != null)
            {
                using var uow = _unitOfWork().Value;
                var oldDirectories = await uow.DirectoryInfoRepository.GetAllAsync();
                var notExistsDirectories = oldDirectories.Where(x => newDirectories.All(y => y.Path != x.Path)).Skip(1);
                await uow.DirectoryInfoRepository.InsertMultipleAsync(newDirectories);
                await uow.DirectoryInfoRepository.DeleteAsync(notExistsDirectories);
                uow.Commit();

                await ViewLoadedAsync();
            }
            IsBusying = false;
        }

        private async Task RefreshFileListAsync()
        {
            IsBusying = true;
            var newFiles = await _fileService.GetFilesAsync(SelectedDirectoryPath);
            if (newFiles != null)
            {
                using var uow = _unitOfWork().Value;
                var oldFiles = await uow.FileInfoRepository.GetAsync(SelectedDirectoryPath);
                var notExistsFiles = oldFiles.Where(x => newFiles.All(y => y.Path != x.Path));
                await uow.FileInfoRepository.InsertMultipleAsync(newFiles);
                await uow.FileInfoRepository.DeleteAsync(notExistsFiles);
                uow.Commit();

                await LoadFilesAsync(null, new ExplorerSelectedMessage(SelectedDirectoryPath));
            }
            IsBusying = false;
        }

        private void SelectedFileChanged(FileInfoModel fileInfo)
        {
            WeakReferenceMessenger.Default.Send(new SelectedFileMessage(fileInfo));
        }

        private void FileClick()
        {
            _fileService.OpenFile(SelectedFile.Path);
        }

        private async Task ScrollToButton()
        {
            if (_page * _limit > _total) return;

            IsBusying = true;
            _page += 1;

            using var uow = _unitOfWork().Value;
            var filesInfo = await uow.FileInfoRepository.GetByPaginationAsync(SelectedDirectoryPath, _limit, _limit * (_page - 1));
            uow.Commit();

            FileInfoDto.AddRange(await  _fileService.GetFileThumbnailAsync(filesInfo));
            IsBusying = false;
        }
    }
}
