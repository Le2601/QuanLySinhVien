using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLySinhVien.Models
{
    [Table("ExerciseContent")]
    public class ExerciseContent
    {

        //public ExerciseContent() {

        //    UploadAssignment = new HashSet<UploadAssignment>();
            
        //}
        
        [Key]
        public int Id { get; set; }

        public int CourseContentId { get; set; }

        public string Alias { get; set; }


        public string Title { get; set; }

        public string Description { get; set; }

        public int Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; } 


        public virtual CourseContent Coursecontent { get; set; }

       
        public virtual ICollection<UploadAssignment> UploadAssignment { get; set; }
      


    }
}
