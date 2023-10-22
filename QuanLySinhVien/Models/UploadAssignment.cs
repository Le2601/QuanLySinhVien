
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLySinhVien.Models
{
    [Table("UploadAssignment")]
    public class UploadAssignment
    {

        [Key]
        public int Id { get; set; }

        public int ExerciseContentId { get; set; }

        //public int AccountId { get; set; }

        public string FullName { get; set; }
        [Display(Name = "Mã số sinh viên")]
        public string Mssv {  get; set; }

        
        public string Alias { get; set; }

        public byte[] Data { get; set; }


        [Required(AllowEmptyStrings = true)]
        public string DataName { get; set; }

        public DateTime UpdateDay { get; set; }


        public virtual ExerciseContent ExerciseContent { get; set; }

        //public virtual Account Account { get; set; }





    }
}
