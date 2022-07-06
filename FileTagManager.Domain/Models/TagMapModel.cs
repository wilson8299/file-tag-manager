using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FileTagManager.Domain.Models
{
    [Table("tag_map")]
    public class TagMapModel
    {
        [Key]
        public int Id { get; set; }
        public int FileInfoId { get; set; }
        public int TagId { get; set; }
    }
}
