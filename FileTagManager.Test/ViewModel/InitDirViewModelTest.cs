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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileTagManager.Test.ViewModel
{
    [TestFixture]
    public class InitDirViewModelTest
    {
        private Func<Owned<IUnitOfWork>> _fouow;
        private Mock<IUnitOfWork> _uow;
        private Mock<IFileService> _fileService;
        private List<(FileInfoModel, bool)> _data;

        [SetUp]
        public void SetUp()
        {
            _data = new List<(FileInfoModel, bool)>();

            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.DirectoryInfoRepository).Returns(new Mock<IDirectoryInfoRepository>().Object);
            _uow.Setup(u => u.FileInfoRepository).Returns(new Mock<IFileInfoRepository>().Object);
            _fouow = () => new Owned<IUnitOfWork>(_uow.Object, new Mock<IDisposable>().Object);

            _fileService = new Mock<IFileService>();
            _fileService.Setup(f => f.SigleDirectoryInfo(It.IsAny<string>()));
        }

        [Test]
        public void DirPath_SetValue_Correctly()
        {
            var expected = "dir_path";
            var mocker = new AutoMocker();
            var initDirViewModel = mocker.CreateInstance<InitDirViewModel>();

            initDirViewModel.DirPath = expected;

            Assert.AreEqual(expected, initDirViewModel.DirPath);
        }

        [Test]
        public void ProcessFiles_SetValue_Correctly()
        {
            var mocker = new AutoMocker();
            var initDirViewModel = mocker.CreateInstance<InitDirViewModel>();
            var expected = new LimitedSizeObservableCollection<string>(10) { "ProcessFiles" };

            initDirViewModel.ProcessFiles = expected;

            Assert.AreEqual(expected, initDirViewModel.ProcessFiles);
        }

        [Test]
        public void OpenFileDialogCommand_ReceiveSelectFolderDialogCall_Once()
        {
            var mocker = new AutoMocker();
            var initDirViewModel = mocker.CreateInstance<InitDirViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();

            initDirViewModel.OpenFileDialogCommand.Execute(null);

            openFileDialogService.Verify(o => o.SelectFolderDialog(), Times.Once);
        }

        [Test]
        public void OpenFileDialogCommand_SetDirPath_Correctly()
        {
            var expected = @"C:\";
            var mocker = new AutoMocker();
            var initDirViewModel = mocker.CreateInstance<InitDirViewModel>();
            var openFileDialogService = mocker.GetMock<IOpenFileDialogService>();
            openFileDialogService.Setup(o => o.SelectFolderDialog()).Returns(CommonFileDialogResult.Ok);
            openFileDialogService.Setup(o => o.SelectedPath).Returns(expected);

            initDirViewModel.OpenFileDialogCommand.Execute(null);

            Assert.AreEqual(expected, initDirViewModel.DirPath);
        }

        [Test]
        public async Task LoadFilesAsyncCommand_SetProcessFilesValue_Correctly()
        {
            var expected = "expected";
            _data.Add((new FileInfoModel { Path = expected }, true));
            _fileService.Setup(f => f.GetInfo(It.IsAny<string>())).Returns(_data);

            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            mocker.Use(_fileService);
            var initDirViewModel = mocker.CreateInstance<InitDirViewModel>();

            await initDirViewModel.LoadFilesAsyncCommand.ExecuteAsync(null);

            Assert.AreEqual(expected, initDirViewModel.ProcessFiles.First());
        }

        [Test]
        public async Task LoadFilesAsyncCommand_ReceiveMapperServiceMapSingleAsyncCall_Once()
        {
            _data.Add((new FileInfoModel { }, true));
            _fileService.Setup(f => f.GetInfo(It.IsAny<string>())).Returns(_data);

            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            mocker.Use(_fileService);
            var initDirViewModel = mocker.CreateInstance<InitDirViewModel>();
            var mapperService = mocker.GetMock<IMapperService>();

            await initDirViewModel.LoadFilesAsyncCommand.ExecuteAsync(null);

            mapperService.Verify(m => m.MapSingleAsync<It.IsAnyType, It.IsAnyType>(It.IsAny<It.IsAnyType>()), Times.Once);
        }

        [Test]
        public async Task LoadFilesAsyncCommand_ReceiveDirectoryInfoRepositoryInsertAsyncCall_Once()
        {
            _data.Add((new FileInfoModel { }, false));
            _fileService.Setup(f => f.GetInfo(It.IsAny<string>())).Returns(_data);

            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            mocker.Use(_fileService);
            var initDirViewModel = mocker.CreateInstance<InitDirViewModel>();

            await initDirViewModel.LoadFilesAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.DirectoryInfoRepository.InsertAsync(It.IsAny<DirectoryInfoModel>()), Times.Once);
        }

        [Test]
        public async Task LoadFilesAsyncCommand_ReceiveDirectoryInfoRepositoryInsertAsyncCall_Twice()
        {
            _data.Add((new FileInfoModel { Attribute = "Directory" }, true));
            _fileService.Setup(f => f.GetInfo(It.IsAny<string>())).Returns(_data);

            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            mocker.Use(_fileService);
            var initDirViewModel = mocker.CreateInstance<InitDirViewModel>();

            await initDirViewModel.LoadFilesAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.DirectoryInfoRepository.InsertAsync(It.IsAny<DirectoryInfoModel>()), Times.Exactly(2));
        }

        [Test]
        public async Task LoadFilesAsyncCommand_ReceiveFileInfoRepositoryInsertAsyncCall_Once()
        {
            _data.Add((new FileInfoModel { }, true));
            _fileService.Setup(f => f.GetInfo(It.IsAny<string>())).Returns(_data);

            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            mocker.Use(_fileService);
            var initDirViewModel = mocker.CreateInstance<InitDirViewModel>();

            await initDirViewModel.LoadFilesAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.FileInfoRepository.InsertAsync(It.IsAny<FileInfoModel>()), Times.Once);
        }

        [Test]
        public async Task LoadFilesAsyncCommand_ReceiveCreateContainerViewModelCall_Once()
        {
            _data.Add((new FileInfoModel { }, false));
            _fileService.Setup(f => f.GetInfo(It.IsAny<string>())).Returns(_data);

            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            mocker.Use(_fileService);
            var initDirViewModel = mocker.CreateInstance<InitDirViewModel>();
            var createViewModelService = mocker.GetMock<ICreateViewModelService>();

            await initDirViewModel.LoadFilesAsyncCommand.ExecuteAsync(null);

            createViewModelService.Verify(c => c.CreateContainerViewModel(VCType.HomeView), Times.Once);
        }

        [Test]
        public async Task LoadFilesAsyncCommand_ReceiveConfigurationServiceCall_Once()
        {
            _data.Add((new FileInfoModel { }, false));
            _fileService.Setup(f => f.GetInfo(It.IsAny<string>())).Returns(_data);

            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            mocker.Use(_fileService);
            var initDirViewModel = mocker.CreateInstance<InitDirViewModel>();
            var configurationService = mocker.GetMock<IConfigurationService>();

            await initDirViewModel.LoadFilesAsyncCommand.ExecuteAsync(null);

            configurationService.Verify(c => c.Set("true", "isInit"), Times.Once);
        }
    }
}
