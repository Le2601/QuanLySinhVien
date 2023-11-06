using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLySinhVien.Models
{
    [Table("CourseContent")]
    public class CourseContent
    {
        public CourseContent()
        {

            ExerciseContents = new HashSet<ExerciseContent>();

        }
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseId { get; set; }
        [Required(ErrorMessage = "Không thể bỏ trống")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Không thể bỏ trống")]
        public string Content { get; set; }

        public string Alias { get; set; }

        public byte[] Data { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string DataName { get; set; }

        public int Location { get; set; }

        [Display(Name = "Bài tập thực hành")]
        public bool IsUpload { get; set; }

        public virtual Course Course { get; set; }


       


        public virtual ICollection<ExerciseContent> ExerciseContents { get; set; }

      

    }
}
