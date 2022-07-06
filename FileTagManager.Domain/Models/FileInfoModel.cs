using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace FileTagManager.Domain.Models
{
    [Table("file_info")]
    public class FileInfoModel : INotifyPropertyChanged
    {
        [Key]
        public int Id { get; set; }
        public string Attribute { get; set; }
        public string Extension { get; set; }
        public string ParentPath { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public byte[] ThumbnailByte { get; set; }
        public ImageSource Thumbnail { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
