using Autofac.Features.OwnedInstances;
using CommunityToolkit.Mvvm.Messaging;
using FileTagManager.Domain.Interfaces;
using FileTagManager.Domain.Interfaces.Repositories;
using FileTagManager.Domain.Models;
using FileTagManager.WPF.Messages;
using FileTagManager.WPF.Services;
using FileTagManager.WPF.ViewModels;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;

namespace FileTagManager.Test.ViewModel
{
    [TestFixture]
    public class FileListViewModelTest
    {
        private Mock<IUnitOfWork> _uow;
        private Func<Owned<IUnitOfWork>> _fouow;

        [SetUp]
        public void SetUp()
        {
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.FileInfoRepository).Returns(new Mock<IFileInfoRepository>().Object);
            _fouow = () => new Owned<IUnitOfWork>(_uow.Object, new Mock<IDisposable>().Object);
        }
        
        public void IsBusying_SetValue_Correctly()
        {
            var expected = true;
            var mocker = new AutoMocker();
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();

            fileListViewModel.IsBusying = expected;

            Assert.AreEqual(expected, fileListViewModel.IsBusying);
        }

        [Test]
        public void SelectedFile_SetValue_Correctly()
        {
            var expected = new FileInfoModel();
            var mocker = new AutoMocker();
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();

            fileListViewModel.SelectedFile = expected;

            Assert.AreEqual(expected, fileListViewModel.SelectedFile);
        }

        [Test]
        public void DirectoryDto_SetValue_Correctly()
        {
            var expected = new ObservableCollection<ExplorerVeiwModel>();
            var mocker = new AutoMocker();
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();

            fileListViewModel.DirectoryDto = expected;

            Assert.AreEqual(expected, fileListViewModel.DirectoryDto);
        }

        [Test]
        public void FileInfoDto_SetValue_Correctly()
        {
            var expected = new ObservableCollection<FileInfoModel>();
            var mocker = new AutoMocker();
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();

            fileListViewModel.FileInfoDto = expected;

            Assert.AreEqual(expected, fileListViewModel.FileInfoDto);
        }

        [Test]
        public async Task ViewLoadedAsync_ReceiveDirectoryInfoRepositoryGetAllAsyncCall_Once()
        {
            var data = new List<DirectoryInfoModel>() { new DirectoryInfoModel() };
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();
            _uow.Setup(u => u.DirectoryInfoRepository.GetAllAsync()).ReturnsAsync(data);
            var fileService = mocker.GetMock<IFileService>();
            fileService.Setup(f => f.GetDirectoriesAsync(data, 0)).ReturnsAsync(new ObservableCollection<ExplorerVeiwModel>() { new ExplorerVeiwModel() });

            await fileListViewModel.ViewLoadedAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.DirectoryInfoRepository.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task ViewLoadedAsync_ReceiveGetDirectoriesAsyncCall_Once()
        {
            var data = new List<DirectoryInfoModel>() { new DirectoryInfoModel() };
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();
            _uow.Setup(u => u.DirectoryInfoRepository.GetAllAsync()).ReturnsAsync(data);
            var fileService = mocker.GetMock<IFileService>();
            fileService.Setup(f => f.GetDirectoriesAsync(data, 0)).ReturnsAsync(new ObservableCollection<ExplorerVeiwModel>() { new ExplorerVeiwModel() });

            await fileListViewModel.ViewLoadedAsyncCommand.ExecuteAsync(null);

            fileService.Verify(f => f.GetDirectoriesAsync(data, 0), Times.Once);
        }

        [Test]
        public void LoadFilesAsync_ReceiveFileInfoRepositoryGetRowCountCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();

            WeakReferenceMessenger.Default.Send(new ExplorerSelectedMessage(It.IsAny<string>()));

            _uow.Verify(u => u.FileInfoRepository.GetRowCount(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public void LoadFilesAsync_ReceiveFileInfoRepositoryGetByPaginationAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();

            WeakReferenceMessenger.Default.Send(new ExplorerSelectedMessage(It.IsAny<string>()));

            _uow.Verify(u => u.FileInfoRepository.GetByPaginationAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Test]
        public void LoadFilesAsync_ReceiveFileServiceGetFileThumbnailAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();
            var fileService = mocker.GetMock<IFileService>();

            WeakReferenceMessenger.Default.Send(new ExplorerSelectedMessage(It.IsAny<string>()));

            fileService.Verify(u => u.GetFileThumbnailAsync(It.IsAny<IEnumerable<FileInfoModel>>()), Times.AtLeastOnce);
        }

        [Test]
        public async Task ScrollToButton_ReceiveFileInfoRepositoryGetByPaginationAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();
            typeof(FileListViewModel)
                .GetField("_total", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(fileListViewModel, 99); ;

            await fileListViewModel.ScrollToButtonCommand.ExecuteAsync(null);

            _uow.Verify(u => u.FileInfoRepository.GetByPaginationAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task ScrollToButton_ReceiveFileServiceGetFileThumbnailAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();
            var fileService = mocker.GetMock<IFileService>();
            typeof(FileListViewModel)
                .GetField("_total", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(fileListViewModel, 99);

            await fileListViewModel.ScrollToButtonCommand.ExecuteAsync(null);

            fileService.Verify(f => f.GetFileThumbnailAsync(It.IsAny<IEnumerable<FileInfoModel>>()), Times.Once);
        }

        [Test]
        public async Task ScrollToButton_FileInfoDtoAddRange_Correctly()
        {
            var expected = new ObservableCollection<FileInfoModel>() { new FileInfoModel() };
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var fileListViewModel = mocker.CreateInstance<FileListViewModel>();
            fileListViewModel.FileInfoDto = new ObservableCollection<FileInfoModel>();
            mocker.GetMock<IFileService>()
                .Setup(f => f.GetFileThumbnailAsync(It.IsAny<IEnumerable<FileInfoModel>>()))
                .ReturnsAsync(expected);
            typeof(FileListViewModel)
                .GetField("_total", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(fileListViewModel, 99);

            await fileListViewModel.ScrollToButtonCommand.ExecuteAsync(null);

            Assert.AreEqual(expected, fileListViewModel.FileInfoDto);
        }
    }
}
