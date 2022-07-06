using Autofac.Features.OwnedInstances;
using FileTagManager.Domain.Interfaces;
using FileTagManager.Domain.Interfaces.Repositories;
using FileTagManager.Domain.Models;
using FileTagManager.WPF.Services;
using FileTagManager.WPF.ViewModels;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FileTagManager.Test.ViewModel
{
    [TestFixture]
    public class SearchViewModelTest
    {
        private Mock<IUnitOfWork> _uow;
        private Func<Owned<IUnitOfWork>> _fouow;

        [SetUp]
        public void SetUp()
        {
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.HistoryRepository).Returns(new Mock<IHistoryRepository>().Object);
            _uow.Setup(u => u.CombineRepository).Returns(new Mock<ICombineRepository>().Object);
            _uow.Setup(u => u.TagMapRepository).Returns(new Mock<ITagMapRepository>().Object);
            _fouow = () => new Owned<IUnitOfWork>(_uow.Object, new Mock<IDisposable>().Object);
        }

        [Test]
        public void IsBusying_SetValue_Correctly()
        {
            var expected = true;
            var mocker = new AutoMocker();
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();

            searchViewModel.IsBusying = expected;

            Assert.AreEqual(expected, searchViewModel.IsBusying);
        }

        [Test]
        public void SearchText_SetValue_Correctly()
        {
            var expected = "search";
            var mocker = new AutoMocker();
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();

            searchViewModel.SearchText = expected;

            Assert.AreEqual(expected, searchViewModel.SearchText);
        }

        [Test]
        public void SearchHistory_SetValue_Correctly()
        {
            var expected = new LimitedSizeObservableCollection<HistoryModel>(0);
            var mocker = new AutoMocker();
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();

            searchViewModel.SearchHistory = expected;

            Assert.AreEqual(expected, searchViewModel.SearchHistory);
        }

        [Test]
        public void FileInfoDto_SetValue_Correctly()
        {
            var expected = new ObservableCollection<FileInfoModel>();
            var mocker = new AutoMocker();
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();

            searchViewModel.FileInfoDto = expected;

            Assert.AreEqual(expected, searchViewModel.FileInfoDto);
        }

        [Test]
        public async Task ViewLoadedAsync_ReceiveHistoryRepositoryGetAllAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();

            await searchViewModel.ViewLoadedAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.HistoryRepository.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task ViewLoadedAsync_SearchHistorySetValue_Correctly()
        {
            var expected = new List<HistoryModel>();
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();
            _uow.Setup(u => u.HistoryRepository.GetAllAsync()).ReturnsAsync(expected);

            await searchViewModel.ViewLoadedAsyncCommand.ExecuteAsync(null);

            Assert.AreEqual(expected, searchViewModel.SearchHistory);
        }

        [Test]
        public async Task SearchAsync_ReceiveHistoryRepositoryInsertAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();
            searchViewModel.SearchText = "search";

            await searchViewModel.SearchAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.HistoryRepository.InsertAsync(It.IsAny<HistoryModel>()), Times.Once);
        }

        [Test]
        public async Task SearchAsync_ReceiveCombineRepositoryGetSearchRowCountAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();
            searchViewModel.SearchText = "search";

            await searchViewModel.SearchAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.CombineRepository.GetSearchRowCountAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [Test]
        public async Task SearchAsync_ReceiveCombineRepositorySearchAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();
            searchViewModel.SearchText = "search";

            await searchViewModel.SearchAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.CombineRepository.SearchAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task SearchAsync_ReceiveMapperServiceMapAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();
            var mapperService = mocker.GetMock<IMapperService>();
            searchViewModel.SearchText = "search";

            await searchViewModel.SearchAsyncCommand.ExecuteAsync(null);

            mapperService.Verify(m => m.MapAsync<It.IsAnyType, It.IsAnyType>(new List<It.IsAnyType>()), Times.Once);
        }

        [Test]
        public async Task SearchAsync_ReceiveFileServiceGetFileThumbnailAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();
            var fileService = mocker.GetMock<IFileService>();
            searchViewModel.SearchText = "search";

            await searchViewModel.SearchAsyncCommand.ExecuteAsync(null);

            fileService.Verify(f => f.GetFileThumbnailAsync(It.IsAny<IEnumerable<FileInfoModel>>()), Times.Once);
        }

        [Test]
        public async Task SearchAsync_SearchHistorySetValue_Correctly()
        {
            var expected = "search";
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();
            searchViewModel.SearchText = expected;

            await searchViewModel.SearchAsyncCommand.ExecuteAsync(null);

            Assert.AreEqual(expected, searchViewModel.SearchHistory.First().Name);
        }

        [Test]
        public async Task HistoryClickAsync_SearchTextSetValue_Correctly()
        {
            var expected = "search";
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();

            await searchViewModel.HistoryClickAsyncCommand.ExecuteAsync(expected);

            Assert.AreEqual(expected, searchViewModel.SearchText);
        }

        [Test]
        public async Task ScrollToButton_ReceiveFileInfoRepositoryGetByPaginationAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();
            typeof(SearchViewModel)
                .GetField("_total", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(searchViewModel, 99);

            await searchViewModel.ScrollToButtonAsyncCommand.ExecuteAsync(null);

            _uow.Verify(u => u.CombineRepository.SearchAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task ScrollToButton_ReceiveMapperServiceMapAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();
            var fileService = mocker.GetMock<IFileService>();
            var mapperService = mocker.GetMock<IMapperService>();
            typeof(SearchViewModel)
                .GetField("_total", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(searchViewModel, 99);

            await searchViewModel.ScrollToButtonAsyncCommand.ExecuteAsync(null);

            mapperService.Verify(m => m.MapAsync<It.IsAnyType, It.IsAnyType>(new List<It.IsAnyType>()), Times.Once);
        }

        [Test]
        public async Task ScrollToButton_ReceiveFileServiceGetFileThumbnailAsyncCall_Once()
        {
            var mocker = new AutoMocker();
            mocker.Use(_fouow);
            var searchViewModel = mocker.CreateInstance<SearchViewModel>();
            var fileService = mocker.GetMock<IFileService>();
            typeof(SearchViewModel)
                .GetField("_total", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(searchViewModel, 99);

            await searchViewModel.ScrollToButtonAsyncCommand.ExecuteAsync(null);

            fileService.Verify(f => f.GetFileThumbnailAsync(It.IsAny<IEnumerable<FileInfoModel>>()), Times.Once);
        }
    }
}
