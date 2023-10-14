
using System;
using System.Collections.Generic;
using QuanLySinhVien.Models;


namespace QuanLySinhVien.ModelView
{
    public class CourseView
    {

        public List<CourseContent> CourseContents {  get; set; } 

        //lay khoa hoc ra trang chu

        public Course course { get; set; }
        public List<Course> Courses { get; set; }

    }
}
