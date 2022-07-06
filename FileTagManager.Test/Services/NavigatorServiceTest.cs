using FileTagManager.WPF.Services;
using FileTagManager.WPF.ViewModels;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace FileTagManager.Test.Services
{
    [TestFixture]
    public class NavigatorServiceTest
    {
        [Test]
        public void NViewContainerNavigator_SetValue_Correctly()
        {
            var mocker = new AutoMocker();
            var navigatorService = mocker.CreateInstance<NavigatorService>();
            var baseViewModel = new Mock<BaseViewModel>();
            var expected = baseViewModel.Object;

            navigatorService.ViewContainerNavigator = expected;

            Assert.AreEqual(expected, navigatorService.ViewContainerNavigator);
        }

        [Test]
        public void ViewDetailNavigator_SetValue_Correctly()
        {
            var mocker = new AutoMocker();
            var navigatorService = mocker.CreateInstance<NavigatorService>();
            var baseViewModel = new Mock<BaseViewModel>();
            var expected = baseViewModel.Object;

            navigatorService.ViewDetailNavigator = expected;

            Assert.AreEqual(expected, navigatorService.ViewDetailNavigator);
        }
    }
}
