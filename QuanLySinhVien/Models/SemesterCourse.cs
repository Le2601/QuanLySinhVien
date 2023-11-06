using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLySinhVien.Models
{
    [Table("SemesterCourse")]
    public class SemesterCourse
    {
        public SemesterCourse()
        {
            Course = new HashSet<Course>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Không thể bỏ trống")]
        public string Title { get; set; }

        public string Alias { get; set; }

        public virtual ICollection<Course> Course { get; set; }
    }
}
