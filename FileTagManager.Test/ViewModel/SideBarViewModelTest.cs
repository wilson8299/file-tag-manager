using Autofac.Features.OwnedInstances;
using FileTagManager.Domain.Interfaces;
using FileTagManager.Domain.Interfaces.Repositories;
using FileTagManager.Domain.Models;
using FileTagManager.WPF.Services;
using FileTagManager.WPF.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FileTagManager.Test.ViewModel
{
    [TestFixture]
    public class SideBarViewModelTest
    {
        private Mock<IUnitOfWork> _uow;
        private Func<Owned<IUnitOfWork>> _fouow;

        [SetUp]
        public void SetUp()
        {
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.FileInfoFTSRepository).Returns(new Mock<IFileInfoFTSRepository>().Object);
            _uow.Setup(u => u.TagRepository).Returns(new Mock<ITagRepository>().Object);
            _uow.Setup(u => u.TagMapRepository).Returns(new Mock<ITagMapRepository>().Object);
            _uow.Setup(u => u.FileInfoRepository).Returns(new Mock<IFileInfoRepository>().Object);
            _uow.Setup(u => u.CombineRepository).Returns(new Mock<ICombineRepository>().Object);
            _uow.Setup(u => u.HistoryRepository).Returns(new Mock<IHistoryRepository>().Object);
            _uow.Setup(u => u.DirectoryInfoRepository).Returns(new Mock<IDirectoryInfoRepository>().Object);
            _fouow = () => new Owned<IUnitOfWork>(_uow.Object, new Mock<IDisposable>().Object);
        }

        [Test]
        public void NavToFileList_ReceiveCreateDetailViewModelCall_Once()
        {
            var mocker = new AutoMocker();
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var createViewModelService = mocker.GetMock<ICreateViewModelService>();

            sideBarViewModel.NavToFileListCommand.Execute(null);

            createViewModelService.Verify(c => c.CreateDetailViewModel(It.IsAny<VDType>()));
        }

        [Test]
        public void NavToSearch_ReceiveCreateDetailViewModelCall_Once()
        {
            var mocker = new AutoMocker();
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var createViewModelService = mocker.GetMock<ICreateViewModelService>();

            sideBarViewModel.NavToSearchCommand.Execute(null);

            createViewModelService.Verify(c => c.CreateDetailViewModel(It.IsAny<VDType>()));
        }

        [Test]
        public async Task ImportAsync_ReceiveSelectFileDialogCall_Once()
        {
            var mocker = new AutoMocker();
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();

            await sideBarViewModel.ImportAsyncCommand.ExecuteAsync(null);

            openFileDialogService.Verify(o => o.SelectFileDialog(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ImportAsync_ReceiveFileInfoFTSRepositoryGetByNameCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            var fileService = mocker.GetMock<IFileService>();
            openFileDialogService.Setup(o => o.SelectFileDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(CommonFileDialogResult.Ok);
            fileService.Setup(f => f.ReadFile(It.IsAny<string>())).Returns("[{\"Name\":\"bird\"}]");

            await sideBarViewModel.ImportAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.FileInfoFTSRepository.GetByName(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ImportAsync_ReceiveFileInfoRepositoryUpdateAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            var fileService = mocker.GetMock<IFileService>();
            openFileDialogService.Setup(o => o.SelectFileDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(CommonFileDialogResult.Ok);
            fileService.Setup(f => f.ReadFile(It.IsAny<string>())).Returns("[{\"Name\":\"bird\"}]");
            _uow.Setup(u => u.FileInfoFTSRepository.GetByName(It.IsAny<string>())).ReturnsAsync(new FileInfoFTSModel());

            await sideBarViewModel.ImportAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.FileInfoRepository.UpdateAsync(It.IsAny<FileInfoModel>()), Times.Once);
        }

        [Test]
        public async Task ImportAsync_ReceiveTagRepositoryInsertAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            var fileService = mocker.GetMock<IFileService>();
            openFileDialogService.Setup(o => o.SelectFileDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(CommonFileDialogResult.Ok);
            fileService.Setup(f => f.ReadFile(It.IsAny<string>())).Returns("[{\"Name\":\"bird\", \"Tag\":\"tag\"}]");
            _uow.Setup(u => u.FileInfoFTSRepository.GetByName(It.IsAny<string>())).ReturnsAsync(new FileInfoFTSModel());

            await sideBarViewModel.ImportAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.TagRepository.InsertAsync(It.IsAny<TagModel>()), Times.Once);
        }

        [Test]
        public async Task ImportAsync_ReceiveTagRepositoryGetByNameAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            var fileService = mocker.GetMock<IFileService>();
            openFileDialogService.Setup(o => o.SelectFileDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(CommonFileDialogResult.Ok);
            fileService.Setup(f => f.ReadFile(It.IsAny<string>())).Returns("[{\"Name\":\"bird\", \"Tag\":\"tag\"}]");
            _uow.Setup(u => u.FileInfoFTSRepository.GetByName(It.IsAny<string>())).ReturnsAsync(new FileInfoFTSModel());

            await sideBarViewModel.ImportAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.TagRepository.GetByNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ImportAsync_ReceiveTagMapRepositoryInsertAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            var fileService = mocker.GetMock<IFileService>();
            openFileDialogService.Setup(o => o.SelectFileDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(CommonFileDialogResult.Ok);
            fileService.Setup(f => f.ReadFile(It.IsAny<string>())).Returns("[{\"Name\":\"bird\", \"Tag\":\"tag\"}]");
            _uow.Setup(u => u.FileInfoFTSRepository.GetByName(It.IsAny<string>())).ReturnsAsync(new FileInfoFTSModel());

            await sideBarViewModel.ImportAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.TagMapRepository.InsertAsync(It.IsAny<TagMapModel>()), Times.Once);
        }

        [Test]
        public async Task ExportAsync_ReceiveSaveFileDialogCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            openFileDialogService.Setup(o => o.SelectFileDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(CommonFileDialogResult.Ok);

            await sideBarViewModel.ExportAsyncCommand.ExecuteAsync(null);

            openFileDialogService.Verify(o => o.SaveFileDialog(), Times.Once);
        }

        [Test]
        public async Task ExportAsync_ReceiveCombineRepositoryExportAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            openFileDialogService.Setup(o => o.SaveFileDialog()).Returns(CommonFileDialogResult.Ok);

            await sideBarViewModel.ExportAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.CombineRepository.ExportAsync(), Times.Once);
        }

        [Test]
        public async Task ExportAsync_ReceiveFileServiceWriteFileAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            var fileService = mocker.GetMock<IFileService>();
            openFileDialogService.Setup(o => o.SaveFileDialog()).Returns(CommonFileDialogResult.Ok);

            await sideBarViewModel.ExportAsyncCommand.ExecuteAsync(null);

            fileService.Verify(f => f.WriteFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ResetAsync_ReceiveDeleteAllAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();

            await sideBarViewModel.ResetAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.HistoryRepository.DeleteAllAsync(), Times.Once);
            _uow.Verify(u => u.TagMapRepository.DeleteAllAsync(), Times.Once);
            _uow.Verify(u => u.FileInfoRepository.DeleteAllAsync(), Times.Once);
            _uow.Verify(u => u.DirectoryInfoRepository.DeleteAllAsync(), Times.Once);
        }

        [Test]
        public async Task ResetAsync_ReceiveCreateContainerViewModelCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var createViewModelService = mocker.GetMock<ICreateViewModelService>();

            await sideBarViewModel.ResetAsyncCommand.ExecuteAsync(null);

            createViewModelService.Verify(c => c.CreateContainerViewModel(It.IsAny<VCType>()), Times.Once);
        }

        [Test]
        public async Task ResetAsync_ReceiveConfigurationServiceSetCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var configurationService = mocker.GetMock<IConfigurationService>();

            await sideBarViewModel.ResetAsyncCommand.ExecuteAsync(null);

            configurationService.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
