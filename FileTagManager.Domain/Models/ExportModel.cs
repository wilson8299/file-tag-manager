using System.Collections.Generic;

namespace FileTagManager.Domain.Models
{
    public class ExportModel
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public byte[] ThumbnailByte { get; set; }
    }
}
