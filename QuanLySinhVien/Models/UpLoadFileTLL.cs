using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLySinhVien.Models
{
    [Table("UpLoadFileTLL")]
    public class UpLoadFileTLL
    {
        [Key]
        public int Id { get; set; }

        public int CourseId { get; set; }


        //dung de diem danh // thuc hien chuc nang diem danh thi cap nhat lai thuoc tinh nay
        //neu co tick vao thi se tang len 1 // co tick vao la vắng

        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Data { get; set; }


        public virtual Course Course { get; set; }


    }
}
