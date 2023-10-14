using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLySinhVien.Models
{
    [Table("CourseMember")]
    public class CourseMember
    {
        [Key]
        public int Id { get; set; }

        public int CourseId { get; set; }

        //public int AccountId { get; set; }


        //dung de diem danh // thuc hien chuc nang diem danh thi cap nhat lai thuoc tinh nay
        //neu co tick vao thi se tang len 1 // co tick vao la vắng

        public int? Attendance { get; set; }

        //thong ting cua sinh vien

        public string Name { get; set; }

        public string Mssv { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

     

        public virtual Course Course { get; set; }

        //public virtual Account Account { get; set; }


    }
}
