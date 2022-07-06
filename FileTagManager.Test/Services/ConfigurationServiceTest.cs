using FileTagManager.WPF.Services;
using Moq.AutoMock;
using NUnit.Framework;
using System.Configuration;

namespace FileTagManager.Test.Services
{
    [TestFixture]
    public class ConfigurationServiceTest
    {
        [Test]
        public void Get_GetAppSetting_ReturnTure()
        {
            var mocker = new AutoMocker();
            var configService = mocker.CreateInstance<ConfigurationService>();

            bool value = configService.Get(bool.Parse, "isInit", true);

            Assert.AreEqual(true, value);
        }

        [Test]
        public void Get_GetUndefinedAppSetting_ReturnFalse()
        {
            var mocker = new AutoMocker();
            var configService = mocker.CreateInstance<ConfigurationService>();

            bool value = configService.Get(bool.Parse, "notfound", false);

            Assert.AreEqual(false, value);
        }

        [Test]
        public void Set_SetAppSetting_Correctly()
        {
            var mocker = new AutoMocker();
            var configService = mocker.CreateInstance<ConfigurationService>();

            configService.Set("true", "isInit");

            Assert.AreEqual("true", ConfigurationManager.AppSettings["isInit"]);
        }

        [Test]
        public void Set_SetUndefinedAppSetting_ReturnVoid()
        {
            var mocker = new AutoMocker();
            var configService = mocker.CreateInstance<ConfigurationService>();

            configService.Set("true", "notfound");

            Assert.IsTrue(true);
        }
    }
}