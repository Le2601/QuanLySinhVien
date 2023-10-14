using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace QuanLySinhVien.Models
{
    [Table("CourseContentFiles")]
    public class CourseContentFiles
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseContentId { get; set; }

        public string Title { get; set; }

        public byte[] Data { get; set; }

        public virtual CourseContent CourseContent { get; set; }
    }
}
