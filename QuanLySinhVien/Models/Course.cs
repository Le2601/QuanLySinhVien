using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace QuanLySinhVien.Models
{
    [Table("Course")]
    public class Course
    {

        public Course()
        {
            CourseMember = new HashSet<CourseMember>();


          
        }

       


        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Không thể bỏ trống")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Không thể bỏ trống")]
        public int AccountId { get; set; }

        public string Alias { get; set; }

        public string Password { get; set; }

        public string Description { get; set; }
        [Required(ErrorMessage = "Không thể bỏ trống")]
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Không thể bỏ trống")]
        public int SemesterCourseId { get; set; }


        [Display(Name = "Ngày bắt đầu khóa học")]
        public DateTime? DateStart { get; set; }

        [Display(Name = "Ngày kết thúc khóa học")]
        public DateTime? DateEnd { get; set; }

        public virtual Account Account { get; set; }


        public virtual department department { get; set; }

        public virtual SemesterCourse SemesterCourse { get; set; }


        public virtual ICollection<CourseMember> CourseMember { get; set; }

       


        public virtual ICollection<CourseContent> CourseContents { get; set; }

        //them table Upload tai lieu 
        //thuoc tinh : id , idcourse , title , nội dung tài liệu , create_at , update_at
    }
}
