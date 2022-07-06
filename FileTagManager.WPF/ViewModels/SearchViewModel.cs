using Autofac.Features.OwnedInstances;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileTagManager.Domain.Interfaces;
using FileTagManager.Domain.Models;
using FileTagManager.WPF.Helpers;
using FileTagManager.WPF.Messages;
using FileTagManager.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FileTagManager.WPF.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly IFileService _fileService;
        private readonly IMapperService _mapperService;
        private readonly Func<Owned<IUnitOfWork>> _unitOfWork;

        private string _searchFileName;
        private IEnumerable<string> _searchTagName;
        private int _limit = 30;
        private int _page = 1;
        private int _total = 0;

        private bool _isBusying;
        public bool IsBusying
        {
            get => _isBusying;
            set => SetProperty(ref _isBusying, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
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

        private LimitedSizeObservableCollection<HistoryModel> _searchHistory;
        public LimitedSizeObservableCollection<HistoryModel> SearchHistory
        {
            get => _searchHistory;
            set => SetProperty(ref _searchHistory, value);
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
        
        private IAsyncRelayCommand _searchAsyncCommand;
        public IAsyncRelayCommand SearchAsyncCommand =>
            _searchAsyncCommand ??= new AsyncRelayCommand(SearchAsync);

        private IAsyncRelayCommand _historyClickAsyncCommand;
        public IAsyncRelayCommand HistoryClickAsyncCommand =>
            _historyClickAsyncCommand ??= new AsyncRelayCommand<string>(HistoryClickAsync);

        private IAsyncRelayCommand _scrollToButtonAsyncCommand;
        public IAsyncRelayCommand ScrollToButtonAsyncCommand =>
            _scrollToButtonAsyncCommand ??= new AsyncRelayCommand(ScrollToButtonAsync);

        private IRelayCommand _fileClickCommand;
        public IRelayCommand FileClickCommand =>
            _fileClickCommand ??= new RelayCommand(FileClick);

        public SearchViewModel(
             IFileService fileService,
             IMapperService mapperService,
             Func<Owned<IUnitOfWork>> unitOfWork)
        {
            _fileService = fileService;
            _mapperService = mapperService;
            _unitOfWork = unitOfWork;

            FileInfoDto = new ObservableCollection<FileInfoModel>();
            SearchHistory = new LimitedSizeObservableCollection<HistoryModel>(5);
            IsBusying = false;
        }

        private async Task ViewLoadedAsync()
        {
            using var uow = _unitOfWork().Value;
            var history = await uow.HistoryRepository.GetAllAsync();
            uow.Commit();

            SearchHistory.AddRange(history);
        }

        private async Task SearchAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return;
            
            IsBusying = true;
            _page = 1;
            var searchText = SearchText.Trim().Split(" ");
            var searchTextSubset = new List<string>() { string.Empty };
            _searchFileName = string.Join(" ", searchText.SkipLast(1).Select(t => t += "* OR").Append(searchText.Last() + "*").ToArray());
            _searchTagName = Subset(searchText);

            using var uow = _unitOfWork().Value;
            await uow.HistoryRepository.InsertAsync(new HistoryModel { Name = SearchText });
            _total = await uow.CombineRepository.GetSearchRowCountAsync(_searchFileName, _searchTagName);
            var data = await uow.CombineRepository.SearchAsync(_searchFileName, _searchTagName,  _limit, _limit * (_page - 1));
            uow.Commit();

            var filesInfo = await _mapperService.MapAsync<FileInfoFTSModel, FileInfoModel>(data);
            FileInfoDto = await _fileService.GetFileThumbnailAsync(filesInfo);
            SearchHistory.Add2First(new HistoryModel { Name = SearchText });
            IsBusying = false;
        }

        private async Task HistoryClickAsync(string name)
        {
            SearchText = name;
            await SearchAsyncCommand.ExecuteAsync(null);
        }

        private void SelectedFileChanged(FileInfoModel fileInfo)
        {
            WeakReferenceMessenger.Default.Send(new SelectedFileMessage(fileInfo));
        }

        private void FileClick()
        {
            _fileService.OpenFile(SelectedFile.Path);
        }

        private async Task ScrollToButtonAsync()
        {
            if (_page * _limit > _total) return;
            IsBusying = true;
            _page += 1;

            using var uow = _unitOfWork().Value;
            var data = await uow.CombineRepository.SearchAsync(_searchFileName, _searchTagName,  _limit, _limit * (_page - 1));
            uow.Commit();

            var filesInfo = await _mapperService.MapAsync<FileInfoFTSModel, FileInfoModel>(data);
            FileInfoDto.AddRange(await _fileService.GetFileThumbnailAsync(filesInfo));
            IsBusying = false;
        }

        private List<string> Subset(string[] searchTextArray)
        {
            List<string> result = new List<string>();
            result.Add("");
            foreach (var searchText in searchTextArray)
            {
                var resultSize = result.Count;
                for (int i = 0; i < resultSize; i++)
                {
                    var temp = result[i];
                    temp += " " + searchText;
                    result.Add(temp.TrimStart());
                }
            }
            return result;
        }

        //[Obsolete]
        //private async Task SearchAsync()
        //{
        //    var searchText = SearchText.Trim().Split(" ");

        //    using var uow = _unitOfWork().Value;
        //    var allTags = await uow.TagRepository.GetAllAsync();
        //    var searchFileName = searchText.Except(allTags.Select(t => t.Name));
        //    var searchTagName = searchText.Except(searchFileName);
        //    var tags = await uow.TagRepository.GetByNamesAsync(searchTagName);
        //    var data = from ft in await uow.FileInfoFTSRepository.MatchAsync(searchFileName)
        //                        join tm in await uow.TagMapRepository.GetAllAsync()
        //                        on ft.RowId equals tm.FileInfoId
        //                        where tags.Contains(tm.TagId) && ft.Path.Contains(@"D:\image")
        //                        select ft;
        //    uow.Commit();
        //}
    }
}
