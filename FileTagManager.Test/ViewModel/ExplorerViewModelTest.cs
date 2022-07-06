using FileTagManager.WPF.ViewModels;
using Moq.AutoMock;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace FileTagManager.Test.ViewModel
{
    [TestFixture]
    public class ExplorerViewModelTest
    {
        [Test]
        public void Name_SetValue_Correctly()
        {
            var expected = "name";
            var mocker = new AutoMocker();
            var explorerViewModel = mocker.CreateInstance<ExplorerVeiwModel>();

            explorerViewModel.Name = expected;

            Assert.AreEqual(expected, explorerViewModel.Name);
        }

        [Test]
        public void Path_SetValue_Correctly()
        {
            var expected = "path";
            var mocker = new AutoMocker();
            var explorerViewModel = mocker.CreateInstance<ExplorerVeiwModel>();

            explorerViewModel.Path = expected;

            Assert.AreEqual(expected, explorerViewModel.Path);
        }

        [Test]
        public void Children_SetValue_Correctly()
        {
            var expected = new ObservableCollection<ExplorerVeiwModel>();
            var mocker = new AutoMocker();
            var explorerViewModel = mocker.CreateInstance<ExplorerVeiwModel>();

            explorerViewModel.Children = expected;

            Assert.AreEqual(expected, explorerViewModel.Children);
        }

        [Test]
        public void IsSelected_SetValue_Correctly()
        {
            var expected = true;
            var mocker = new AutoMocker();
            var explorerViewModel = mocker.CreateInstance<ExplorerVeiwModel>();

            explorerViewModel.IsSelected = expected;

            Assert.AreEqual(expected, explorerViewModel.IsSelected);
        }

        [Test]
        public void IsExpanded_SetValue_Correctly()
        {
            var expected = true;
            var mocker = new AutoMocker();
            var explorerViewModel = mocker.CreateInstance<ExplorerVeiwModel>();

            explorerViewModel.IsExpanded = expected;

            Assert.AreEqual(expected, explorerViewModel.IsExpanded);
        }
    }
}
