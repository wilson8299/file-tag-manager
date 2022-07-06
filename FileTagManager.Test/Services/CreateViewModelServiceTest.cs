using FileTagManager.WPF.Services;
using Moq.AutoMock;
using NUnit.Framework;
using System;

namespace FileTagManager.Test.Services
{
    [TestFixture]
    public class CreateViewModelServiceTest
    {
        [Test]
        public void CreateContainerViewModel_CreateInitDirViewModel_ReturnInitDirViewModel()
        {
            var expected = "InitDirViewModel";
            var mocker = new AutoMocker();
            var createViewModelService = mocker.CreateInstance<CreateViewModelService>();

            var actual = createViewModelService.CreateContainerViewModel(VCType.InitDirView);

            Assert.AreEqual(expected, actual.GetType().BaseType.Name);
        }

        [Test]
        public void CreateContainerViewModel_CreateHomeViewModel_ReturnHomeViewModel()
        {
            var expected = "HomeViewModel";
            var mocker = new AutoMocker();
            var createViewModelService = mocker.CreateInstance<CreateViewModelService>();

            var actual = createViewModelService.CreateContainerViewModel(VCType.HomeView);

            Assert.AreEqual(expected, actual.GetType().BaseType.Name);
        }

        [Test]
        public void CreateContainerViewModel_Unexpected_ThrowArgumentException()
        {
            var excepted = "The ViewType does not have a ViewModel. (Parameter 'viewType')";
            var mocker = new AutoMocker();
            var createViewModelService = mocker.CreateInstance<CreateViewModelService>();

            try
            {
                createViewModelService.CreateContainerViewModel((VCType)(-1));
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual(excepted, ex.Message);
            }
        }

        [Test]
        public void CreateDetailViewModel_CreateFileListViewModel_ReturnFileListViewModel()
        {
            var expected = "FileListViewModel";
            var mocker = new AutoMocker();
            var createViewModelService = mocker.CreateInstance<CreateViewModelService>();

            var actual = createViewModelService.CreateDetailViewModel(VDType.FileListView);

            Assert.AreEqual(expected, actual.GetType().BaseType.Name);
        }

        [Test]
        public void CreateDetailViewModel_CreateSearchViewModel_ReturnSearchViewModel()
        {
            var expected = "SearchViewModel";
            var mocker = new AutoMocker();
            var createViewModelService = mocker.CreateInstance<CreateViewModelService>();

            var actual = createViewModelService.CreateDetailViewModel(VDType.SearchView);

            Assert.AreEqual(expected, actual.GetType().BaseType.Name);

        }

        [Test]
        public void CreateDetailViewModel_Unexpected_ThrowArgumentException()
        {
            var excepted = "The ViewType does not have a ViewModel. (Parameter 'viewType')";
            var mocker = new AutoMocker();
            var createViewModelService = mocker.CreateInstance<CreateViewModelService>();

            try
            {
                createViewModelService.CreateDetailViewModel((VDType)(-1));
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual(excepted, ex.Message);
            }
        }
    }
}
