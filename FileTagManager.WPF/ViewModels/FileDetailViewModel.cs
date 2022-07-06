using Autofac.Features.OwnedInstances;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileTagManager.Domain.Interfaces;
using FileTagManager.Domain.Models;
using FileTagManager.WPF.Helpers;
using FileTagManager.WPF.Messages;
using FileTagManager.WPF.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FileTagManager.WPF.ViewModels
{
    public class FileDetailViewModel : BaseViewModel
    {
        private readonly IFileService _fileService;
        private readonly IOpenFileDialogService _openFileDialogService;
        private readonly Func<Owned<IUnitOfWork>> _unitOfWork;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private string _addTagText;
        public string AddTagText
        {
            get => _addTagText;
            set => SetProperty(ref _addTagText, value);
        }

        private FileInfoModel _selectedFile;
        public FileInfoModel SelectedFile
        {
            get => _selectedFile;
            set => SetProperty(ref _selectedFile, value);
        }

        private ObservableCollection<TagModel> _tagDto;
        public ObservableCollection<TagModel> TagDto
        {
            get => _tagDto;
            set => SetProperty(ref _tagDto, value);
        }

        private IAsyncRelayCommand _changeThumbnailAsyncCommand;
        public IAsyncRelayCommand ChangeThumbnailAsyncCommand =>
            _changeThumbnailAsyncCommand ??= new AsyncRelayCommand(ChangeThumbnailAsync);

        private IAsyncRelayCommand _defaultThumbnailAsyncCommand;
        public IAsyncRelayCommand DefaultThumbnailAsyncCommand =>
            _defaultThumbnailAsyncCommand ??= new AsyncRelayCommand(DefaultThumbnailAsync);

        private IAsyncRelayCommand _addTagAsyncCommand;
        public IAsyncRelayCommand AddTagAsyncCommand =>
            _addTagAsyncCommand ??= new AsyncRelayCommand(AddTagAsync);

        private IAsyncRelayCommand _deleteTagAsyncCommand;
        public IAsyncRelayCommand DeleteTagAsyncCommand =>
            _deleteTagAsyncCommand ??= new AsyncRelayCommand<TagModel>(DeleteTagAsync);

        public FileDetailViewModel(IFileService fileService,
            IOpenFileDialogService openFileDialogService,
            Func<Owned<IUnitOfWork>> unitOfWork)
        {
            _fileService = fileService;
            _openFileDialogService = openFileDialogService;
            _unitOfWork = unitOfWork;

            WeakReferenceMessenger.Default.Register<SelectedFileMessage>(this, async (r, m) => await SelectedFileChangeAsync(r, m));
            WeakReferenceMessenger.Default.Register<IsSelectedMessage>(this, (r, m) => IsSelected = m.Value);
        }

        private async Task SelectedFileChangeAsync(object r, SelectedFileMessage m)
        {
            var fileInfo = m.Value;
            if (fileInfo == null) return;

            SelectedFile = fileInfo;
            IsSelected = true;

            using var uow = _unitOfWork().Value;
            var data = await uow.CombineRepository.GetFileTags(SelectedFile.Id);
            uow.Commit();

            TagDto = new ObservableCollection<TagModel>(data);
        }

        #region Thumbnail Method

        private async Task ChangeThumbnailAsync()
        {
            if (_openFileDialogService.SelectFileDialog("Images", "png,gif,jpg,jpeg,bmp") != CommonFileDialogResult.Ok) return;
           
            var imgByte = _fileService.ReadFileByte(_openFileDialogService.SelectedPath);

            using var uow = _unitOfWork().Value;
            await uow.FileInfoRepository.UpdateAsync(new FileInfoModel { Id = SelectedFile.Id, ThumbnailByte = imgByte });
            uow.Commit();

            SelectedFile.Thumbnail = _fileService.GetIThumbnailByByte(imgByte);
        }

        private async Task DefaultThumbnailAsync()
        {
            using var uow = _unitOfWork().Value;
            await uow.FileInfoRepository.UpdateAsync(new FileInfoModel { Id = SelectedFile.Id, ThumbnailByte = null });
            uow.Commit();

            SelectedFile.Thumbnail = _fileService.GetIThumbnailByPath(SelectedFile.Path);
        }

        #endregion

        #region Tag Method

        private async Task AddTagAsync()
        {
            if (string.IsNullOrEmpty(AddTagText)) return;

            var newTagDto = new List<TagModel>();
            var addTagText = AddTagText;
            AddTagText = string.Empty;

            IEnumerable<TagModel> tags = addTagText
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(tag => new TagModel { Name = tag.Trim() })
                .Where(tag => !string.IsNullOrEmpty(tag.Name) && !TagDto.Any(tagDto => tagDto.Name == tag.Name));

            using var uow = _unitOfWork().Value;
            foreach (var tag in tags)
            {
                var tagId = await uow.TagRepository.UpsertAsync(tag);
                newTagDto.Add(new TagModel { Id = tagId, Name = tag.Name });
                await uow.TagMapRepository.InsertAsync(new TagMapModel { FileInfoId = SelectedFile.Id, TagId = tagId });
            }
            uow.Commit();

            TagDto.AddRange(newTagDto);
        }

        private async Task DeleteTagAsync(TagModel tag)
        {
            using var uow = _unitOfWork().Value;
            await uow.TagMapRepository.DeleteAsync(new TagMapModel { TagId = tag.Id, FileInfoId = SelectedFile.Id });
            uow.Commit();

            TagDto.Remove(tag);
        }

        #endregion

    }
}
