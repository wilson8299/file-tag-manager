using FileTagManager.WPF.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using Moq;
using NUnit.Framework;

namespace FileTagManager.Test.Services
{
    [TestFixture]
    public class OpenFileDialogServiceTest
    {
        [Test]
        public void SelectedPath_SetSelectedPath_Correctly()
        {
            var expected = "expected";
            var openFileDialogService = new Mock<IOpenFileDialogService>();
            openFileDialogService.Setup(o => o.SelectedPath).Returns(expected);

            Assert.AreEqual(expected, openFileDialogService.Object.SelectedPath);
        }

        [Test]
        public void SelectedSavePath_SetSelectedSavePath_Correctly()
        {
            var expected = "expected";
            var openFileDialogService = new Mock<IOpenFileDialogService>();
            openFileDialogService.Setup(o => o.SelectedSavePath).Returns(expected);

            Assert.AreEqual(expected, openFileDialogService.Object.SelectedSavePath);
        }

        [Test]
        public void SelectFolderDialog_OpenFileDialog_ReturnOk()
        {
            var openFileDialogService = new Mock<IOpenFileDialogService>();
            openFileDialogService.Setup(o => o.SelectFolderDialog()).Returns(CommonFileDialogResult.Ok);

            var showDialog = openFileDialogService.Object.SelectFolderDialog();

            Assert.AreEqual(CommonFileDialogResult.Ok, showDialog);
        }

        [Test]
        public void SelectFileDialog_OpenFileDialogWithParameter_ReturnOk()
        {
            var openFileDialogService = new Mock<IOpenFileDialogService>();
            openFileDialogService.Setup(o => o.SelectFileDialog(default, default)).Returns(CommonFileDialogResult.Ok);

            var showDialog = openFileDialogService.Object.SelectFileDialog(default, default);

            Assert.AreEqual(CommonFileDialogResult.Ok, showDialog);
        }

        [Test]
        public void SaveFileDialog_OpenSaveFileDialog_ReturnOk()
        {
            var openFileDialogService = new Mock<IOpenFileDialogService>();
            openFileDialogService.Setup(o => o.SaveFileDialog()).Returns(CommonFileDialogResult.Ok);

            var showDialog = openFileDialogService.Object.SaveFileDialog();

            Assert.AreEqual(CommonFileDialogResult.Ok, showDialog);
        }
    }
}
