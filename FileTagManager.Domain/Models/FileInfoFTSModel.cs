namespace FileTagManager.Domain.Models
{
    public class FileInfoFTSModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public byte[] ThumbnailByte { get; set; }
    }
}
