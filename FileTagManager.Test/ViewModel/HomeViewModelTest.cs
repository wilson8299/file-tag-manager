using FileTagManager.WPF.Services;
using FileTagManager.WPF.ViewModels;
using Moq.AutoMock;
using NUnit.Framework;

namespace FileTagManager.Test.ViewModel
{
    [TestFixture]
    public class HomeViewModelTest
    {
        [Test]
        public void SideBarViewModel_SetValue_Correctly()
        {
            var mocker = new AutoMocker();
            var sideBarViewModel = mocker.CreateInstance<SideBarViewModel>();
            var homeViewModel = mocker.CreateInstance<HomeViewModel>();

            homeViewModel.SideBarViewModel = sideBarViewModel;

            Assert.AreEqual(sideBarViewModel, homeViewModel.SideBarViewModel);
        }

        [Test]
        public void FileListViewModel_SetValue_Correctly()
        {
            var mocker = new AutoMocker();
            var fileDetailViewModel = mocker.CreateInstance<FileDetailViewModel>();
            var homeViewModel = mocker.CreateInstance<HomeViewModel>();

            homeViewModel.FileDetailViewModel = fileDetailViewModel;

            Assert.AreEqual(fileDetailViewModel, homeViewModel.FileDetailViewModel);
        }

        [Test]
        public void Navigator_SetValue_Correctly()
        {
            var mocker = new AutoMocker();
            var navigatorService = mocker.CreateInstance<NavigatorService>();
            var homeViewModel = mocker.CreateInstance<HomeViewModel>();

            homeViewModel.Navigator = navigatorService;

            Assert.AreEqual(navigatorService, homeViewModel.Navigator);
        }
    }
}
