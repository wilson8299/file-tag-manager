using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileTagManager.Domain.Models
{
    [Table("directory_info")]
    public class DirectoryInfoModel
    {
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public string ParentPath { get; set; }
    }
}
