using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileTagManager.Domain.Models
{
    [Table("history")]
    public class HistoryModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
