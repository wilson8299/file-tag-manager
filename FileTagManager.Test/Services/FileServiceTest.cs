using FileTagManager.Domain.Models;
using FileTagManager.WPF.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

namespace FileTagManager.Test.Services
{
    [TestFixture]
    public class FileServiceTest
    {
        [Test]
        public void GetInfo_Correctly_RetrunDirectoryInfoAndFileInfo()
        {
            var path = @"D:\Test";
            var data = new List<string>() { $@"{path}\test.txt", $@"{path}\test2.txt" };
            var mockFileDictionary = data.ToDictionary(str => str, str => new MockFileData(str));
            var mockFileSystem = new MockFileSystem(mockFileDictionary);
            var fileService = new FileService(mockFileSystem);

            var files = fileService.GetInfo(path);

            CollectionAssert.AreEquivalent(files.Select(d => d.Item1.Path), data);
        }

        [Test]
        public void GetDirectoriesTree_Correctly_ReturnDirectoryCollection()
        {
            var mockFileSystem = new Mock<IFileSystem>();
            var fileService = new FileService(mockFileSystem.Object);
            var directoryInfo = new List<DirectoryInfoModel> { new DirectoryInfoModel { Id = -1, ParentId = 0 , Name = "test", ParentPath = "parentPath", Path = "path",} };

            var directories = fileService.GetDirectoriesTree(directoryInfo, 0);

            Assert.AreEqual(directories.First().Name, directoryInfo.First().Name);
        }

        [Test]
        public void GetFileThumbnail_GetThumbnailByPath_ValueNotNull()
        {
            var directoryInfo = new List<FileInfoModel> { new FileInfoModel { Path = @"C:\" } };
            var mockFileSystem = new Mock<IFileSystem>();
            var fileService = new FileService(mockFileSystem.Object);

            var fileInfo = fileService.GetFileThumbnail(directoryInfo);

            Assert.AreNotEqual(null, fileInfo.First().Thumbnail);
        }
    }
}
