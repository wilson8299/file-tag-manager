using Autofac.Features.OwnedInstances;
using CommunityToolkit.Mvvm.Messaging;
using FileTagManager.Domain.Interfaces;
using FileTagManager.Domain.Interfaces.Repositories;
using FileTagManager.Domain.Models;
using FileTagManager.WPF.Messages;
using FileTagManager.WPF.Services;
using FileTagManager.WPF.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FileTagManager.Test.ViewModel
{
    [TestFixture]
    public class FileDetailViewModelTest
    {
        private Mock<IUnitOfWork> _uow;
        private Func<Owned<IUnitOfWork>> _fouow;

        [SetUp]
        public void SetUp()
        {
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.TagRepository).Returns(new Mock<ITagRepository>().Object);
            _uow.Setup(u => u.TagMapRepository).Returns(new Mock<ITagMapRepository>().Object);
            _uow.Setup(u => u.CombineRepository).Returns(new Mock<ICombineRepository>().Object);
            _uow.Setup(u => u.FileInfoRepository).Returns(new Mock<IFileInfoRepository>().Object);
            _fouow = () => new Owned<IUnitOfWork>(_uow.Object, new Mock<IDisposable>().Object);
        }

        [Test]
        public void IsSelected_SetValue_Correctly()
        {
            var expected = true;
            var mocker = new AutoMocker();
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();

            fileDetailViewModel.IsSelected = expected;

            Assert.AreEqual(expected, fileDetailViewModel.IsSelected);
        }

        [Test]
        public void AddTagText_SetValue_Correctly()
        {
            var expected = "tag";
            var mocker = new AutoMocker();
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();

            fileDetailViewModel.AddTagText = expected;

            Assert.AreEqual(expected, fileDetailViewModel.AddTagText);
        }

        [Test]
        public void SelectedFile_SetValue_Correctly()
        {
            var expected = new FileInfoModel();
            var mocker = new AutoMocker();
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();

            fileDetailViewModel.SelectedFile = expected;

            Assert.AreEqual(expected, fileDetailViewModel.SelectedFile);
        }

        [Test]
        public void TagDto_SetValue_Correctly()
        {
            var expected = new ObservableCollection<TagModel>();
            var mocker = new AutoMocker();
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();

            fileDetailViewModel.TagDto = expected;

            Assert.AreEqual(expected, fileDetailViewModel.TagDto);
        }

        
        [Test]
        public void SelectedFileChangeAsync_SelectedFileSetValue_Correctly()
        {
            var expected = new FileInfoModel();
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();

            WeakReferenceMessenger.Default.Send(new SelectedFileMessage(expected));

            Assert.AreEqual(expected, fileDetailViewModel.SelectedFile);
        }

        [Test]
        public void SelectedFileChangeAsync_IsSelectedSetValue_Correctly()
        {
            var expected = true;
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();

            WeakReferenceMessenger.Default.Send(new SelectedFileMessage(new FileInfoModel()));

            Assert.AreEqual(expected, fileDetailViewModel.IsSelected);
        }

        [Test]
        public void SelectedFileChangeAsync_ReceiveCombineRepositoryGetFileTagsCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();

            WeakReferenceMessenger.Default.Send(new SelectedFileMessage(new FileInfoModel()));

            _uow.Verify(u => u.CombineRepository.GetFileTags(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Test]
        public void SelectedFileChangeAsync_TagDtoSetValue_Correctly()
        {
            var expected = new List<TagModel>() { new TagModel { Id = 0 } };
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();
            _uow.Setup(u => u.CombineRepository.GetFileTags(It.IsAny<int>())).ReturnsAsync(expected);

            WeakReferenceMessenger.Default.Send(new SelectedFileMessage(new FileInfoModel()));

            Assert.AreEqual(expected, fileDetailViewModel.TagDto);
        }

        [Test]
        public async Task ChangeThumbnailAsync_ReceiveOpenFileDialogServiceSelectFileDialogCall_Once()
        {
            var mocker = new AutoMocker();
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();

            await fileDetailViewModel.ChangeThumbnailAsyncCommand.ExecuteAsync(null);

            openFileDialogService.Verify(o => o.SelectFileDialog(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ChangeThumbnailAsync_ReceiveFileServiceReadFileByteCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            var fileService = mocker.GetMock<IFileService>();
            openFileDialogService.Setup(o => o.SelectFileDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(CommonFileDialogResult.Ok);
            fileDetailViewModel.SelectedFile = new FileInfoModel();

            await fileDetailViewModel.ChangeThumbnailAsyncCommand.ExecuteAsync(null);

            fileService.Verify(f => f.ReadFileByte(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ChangeThumbnailAsync_ReceiveFileInfoRepositoryUpdateAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            openFileDialogService.Setup(o => o.SelectFileDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(CommonFileDialogResult.Ok);
            fileDetailViewModel.SelectedFile = new FileInfoModel();

            await fileDetailViewModel.ChangeThumbnailAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.FileInfoRepository.UpdateAsync(It.IsAny<FileInfoModel>()), Times.Once);
        }

        [Test]
        public async Task ChangeThumbnailAsync_ReceiveFileServiceGetIThumbnailByByteCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            var fileService = mocker.GetMock<IFileService>();
            openFileDialogService.Setup(o => o.SelectFileDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(CommonFileDialogResult.Ok);
            fileDetailViewModel.SelectedFile = new FileInfoModel();

            await fileDetailViewModel.ChangeThumbnailAsyncCommand.ExecuteAsync(null);

            fileService.Verify(f => f.GetIThumbnailByByte(It.IsAny<byte[]>()), Times.Once);
        }

        [Test]
        public async Task DefaultThumbnailAsync_ReceiveFileInfoRepositoryUpdateAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            fileDetailViewModel.SelectedFile = new FileInfoModel();

            await fileDetailViewModel.DefaultThumbnailAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.FileInfoRepository.UpdateAsync(It.IsAny<FileInfoModel>()), Times.Once);
        }

        [Test]
        public async Task DefaultThumbnailAsync_ReceiveFileServiceGetIThumbnailByPathCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            var fileService = mocker.GetMock<IFileService>();
            fileDetailViewModel.SelectedFile = new FileInfoModel();

            await fileDetailViewModel.DefaultThumbnailAsyncCommand.ExecuteAsync(null);

            fileService.Verify(f => f.GetIThumbnailByPath(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task AddTagAsync_ReceiveTagRepositoryUpsertAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            fileDetailViewModel.SelectedFile = new FileInfoModel();
            fileDetailViewModel.AddTagText = "tag";
            fileDetailViewModel.TagDto = new ObservableCollection<TagModel>();

            await fileDetailViewModel.AddTagAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.TagRepository.UpsertAsync(It.IsAny<TagModel>()), Times.Once);
        }

        [Test]
        public async Task AddTagAsync_ReceiveTagMapRepositoryInsertAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            fileDetailViewModel.SelectedFile = new FileInfoModel();
            fileDetailViewModel.AddTagText = "tag";
            fileDetailViewModel.TagDto = new ObservableCollection<TagModel>();

            await fileDetailViewModel.AddTagAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.TagMapRepository.InsertAsync(It.IsAny<TagMapModel>()), Times.Once);
        }

        [Test]
        public async Task DeleteTagAsync_ReceiveTagMapRepositoryInsertAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();
            fileDetailViewModel.SelectedFile = new FileInfoModel();
            fileDetailViewModel.TagDto = new ObservableCollection<TagModel>();

            await fileDetailViewModel.DeleteTagAsyncCommand.ExecuteAsync(new TagModel());

            _uow.Verify(u => u.TagMapRepository.DeleteAsync(It.IsAny<TagMapModel>()), Times.Once);
        }
    }
}
