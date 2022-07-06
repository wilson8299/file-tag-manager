using FileTagManager.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileTagManager.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDirectoryInfoRepository DirectoryInfoRepository { get; }
        IFileInfoRepository FileInfoRepository { get; }
        IFileInfoFTSRepository FileInfoFTSRepository { get; }
        ITagRepository TagRepository { get; }
        ITagMapRepository TagMapRepository { get; }
        IHistoryRepository HistoryRepository { get; }
        ICombineRepository CombineRepository { get; }
        void Commit();
    }
}
