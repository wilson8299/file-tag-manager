using FileTagManager.WPF.Services;
using FileTagManager.WPF.ViewModels;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace FileTagManager.Test.ViewModel
{
    [TestFixture]
    public class MainViewModelTest
    {
        [Test]
        public void Navigator_SetValue_Correctly()
        {
            var mocker = new AutoMocker();
            var navigatorService = mocker.CreateInstance<NavigatorService>();
            var mainViewModel = mocker.CreateInstance<MainViewModel>();

            mainViewModel.Navigator = navigatorService;

            Assert.AreEqual(navigatorService, mainViewModel.Navigator);
        }

        [Test]
        public void ViewLoadedCommand_Execute_Correctly()
        {
            var mocker = new AutoMocker();
            var mainViewModel = mocker.CreateInstance<MainViewModel>();
            var createViewModelService = mocker.GetMock<ICreateViewModelService>();
            var configurationService = mocker.GetMock<IConfigurationService>();

            mainViewModel.ViewLoadedCommand.Execute(null);

            configurationService.Verify(c => c.Get(bool.Parse, It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
            createViewModelService.Verify(c => c.CreateContainerViewModel(default), Times.Once);
            createViewModelService.Verify(c => c.CreateDetailViewModel(default), Times.Once);
        }
    }
}
