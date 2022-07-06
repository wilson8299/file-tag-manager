using Autofac.Features.OwnedInstances;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileTagManager.Domain.Interfaces;
using FileTagManager.Domain.Models;
using FileTagManager.WPF.Messages;
using FileTagManager.WPF.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileTagManager.WPF.ViewModels
{
    public class SideBarViewModel : BaseViewModel
    {
        private readonly INavigatorService _navigatorService;
        private readonly ICreateViewModelService _createViewModelService;
        private readonly IOpenFileDialogService _openFileDialogService;
        private readonly IConfigurationService _configurationService;
        private readonly IFileService _fileService;
        private readonly Func<Owned<IUnitOfWork>> _unitOfWork;

        private IRelayCommand _navToFileListCommand;
        public IRelayCommand NavToFileListCommand =>
            _navToFileListCommand ??= new RelayCommand(NavToFileList);

        private IRelayCommand _navToSearchCommand;
        public IRelayCommand NavToSearchCommand =>
            _navToSearchCommand ??= new RelayCommand(NavToSearch);

        private IAsyncRelayCommand _importAsyncCommand;
        public IAsyncRelayCommand ImportAsyncCommand =>
            _importAsyncCommand ??= new AsyncRelayCommand(ImportAsync);

        private IAsyncRelayCommand _exportAsyncCommand;
        public IAsyncRelayCommand ExportAsyncCommand =>
            _exportAsyncCommand ??= new AsyncRelayCommand(ExportAsync);

        private IAsyncRelayCommand _resetAsyncCommand;
        public IAsyncRelayCommand ResetAsyncCommand =>
            _resetAsyncCommand ??= new AsyncRelayCommand(ResetAsync);

        public SideBarViewModel(
            INavigatorService navigatorService,
            ICreateViewModelService createViewModelService,
            IOpenFileDialogService openFileDialogService,
            IConfigurationService configurationService,
            IFileService fileService,
            Func<Owned<IUnitOfWork>> unitOfWork)
        {
            _navigatorService = navigatorService;
            _createViewModelService = createViewModelService;
            _openFileDialogService = openFileDialogService;
            _configurationService = configurationService;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        private void NavToFileList()
        {
            _navigatorService.ViewDetailNavigator = _createViewModelService.CreateDetailViewModel(VDType.FileListView);
            WeakReferenceMessenger.Default.Send(new IsSelectedMessage(false));
        }

        private void NavToSearch()
        {
            _navigatorService.ViewDetailNavigator = _createViewModelService.CreateDetailViewModel(VDType.SearchView);
            WeakReferenceMessenger.Default.Send(new IsSelectedMessage(false));
        }

        private async Task ImportAsync()
        {
            if (_openFileDialogService.SelectFileDialog("Json", "json") != CommonFileDialogResult.Ok) return;

            var files = JsonConvert.DeserializeObject<IEnumerable<ExportModel>>(_fileService.ReadFile(_openFileDialogService.SelectedPath));

            using var uow = _unitOfWork().Value;
            foreach (var file in files ?? Enumerable.Empty<ExportModel>())
            {
                var fts = await uow.FileInfoFTSRepository.GetByName(Regex.Replace(file.Name, @"[\W]+", " "));
                if (fts == null) continue;

                await uow.FileInfoRepository.UpdateAsync(new FileInfoModel { Id = fts.Id, ThumbnailByte = file.ThumbnailByte });
                foreach (var tag in file.Tag?.Split(",") ?? Enumerable.Empty<string>())
                {
                    await uow.TagRepository.InsertAsync(new TagModel { Name = tag });
                    var tagId = await uow.TagRepository.GetByNameAsync(tag);
                    await uow.TagMapRepository.InsertAsync(new TagMapModel { FileInfoId = fts.Id, TagId = tagId });
                }
                uow.Commit();
            }
        }

        private async Task ExportAsync()
        {
            if (_openFileDialogService.SaveFileDialog() != CommonFileDialogResult.Ok) return;

            using var uow = _unitOfWork().Value;
            var data = await uow.CombineRepository.ExportAsync();
            uow.Commit();

            _fileService.WriteFile(_openFileDialogService.SelectedSavePath, JsonConvert.SerializeObject(data));

        }

        private async Task ResetAsync()
        {
            using var uow = _unitOfWork().Value;
            await uow.HistoryRepository.DeleteAllAsync();
            await uow.TagMapRepository.DeleteAllAsync();
            await uow.FileInfoRepository.DeleteAllAsync();
            await uow.DirectoryInfoRepository.DeleteAllAsync();
            uow.Commit();

            _navigatorService.ViewContainerNavigator = _createViewModelService.CreateContainerViewModel(VCType.InitDirView);
            _configurationService.Set("false", "isInit");
        }
    }
}
