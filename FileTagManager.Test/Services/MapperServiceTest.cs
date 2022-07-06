using FileTagManager.Domain.Models;
using FileTagManager.WPF.Services;
using Moq.AutoMock;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FileTagManager.Test.Services
{
    [TestFixture]
    public class MapperServiceTest
    {
        [Test]
        public void Map_MapperOldType_ReturnNewType()
        {
            var expected = "Test";
            var mocker = new AutoMocker();
            var mapperService = mocker.CreateInstance<MapperService>();
            var data = new List<FileInfoFTSModel> { new FileInfoFTSModel { Name = expected } };

            var actual = mapperService.Map<FileInfoFTSModel, FileInfoModel>(data);

            Assert.AreEqual(expected, actual.First().Name);
        }

        [Test]
        public void MapSigle_MapperOldType_ReturnNewType()
        {
            var expected = "Test";
            var mocker = new AutoMocker();
            var mapperService = mocker.CreateInstance<MapperService>();
            var data = new FileInfoFTSModel { Name = expected };

            var actual = mapperService.MapSingle<FileInfoFTSModel, FileInfoModel>(data);

            Assert.AreEqual(expected, actual.Name);
        }
    }
}
