using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace QuanLySinhVien.Models
{
    [Table("Account")]
    public class Account
    {

        public Account()
        {
            Courses = new HashSet<Course>();
            //CourseMember = new HashSet<CourseMember>();

        }

        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }
      
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
      
        public int? RoleId { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Role Role { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<UploadAssignment> UploadAssignment { get; set; }

        //public virtual ICollection<CourseMember> CourseMember { get; set; }

    }
}
