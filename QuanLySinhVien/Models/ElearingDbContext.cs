using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Models
{
    public class ElearingDbContext : DbContext
    {

        public ElearingDbContext(DbContextOptions<ElearingDbContext> options) : base(options)
        {
        
        
        }


        public DbSet<Role> Roles { get; set; }

        public DbSet<Account> Account { get; set; }


        //Khoa
        public DbSet<department> Department { get; set; }


        //Khóa học
         public DbSet<Course> Courses { get; set; }


        //Thành viên có trong khóa học
        public DbSet<CourseMember> CourseMembers { get; set; }



       


        //Năm khóa học

        public DbSet<SemesterCourse> SemesterCourse { get; set; }


        //Nội dung khóa học
        public DbSet<CourseContent> CourseContents { get; set; }

        //Nội dung khóa học dành cho sv upload tài liệu do gv mở
        public DbSet<ExerciseContent> ExerciseContents { get; set; }


        //upload bt danh cho sv

        public DbSet<UploadAssignment> UploadAssignments { get; set; }

     

      



        //relationship cac table

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(150);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

               

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Accounts_Roles");

             

            });

            modelBuilder.Entity<Role>(entity => {

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.RoleName).HasMaxLength(50);


            });

            modelBuilder.Entity<Course>()
                .HasOne(p => p.department)
                .WithMany(c => c.Course)
                .HasForeignKey(p => p.DepartmentId);

            modelBuilder.Entity<CourseMember>()
                .HasOne(p => p.Course)
                .WithMany(c => c.CourseMember)
                .HasForeignKey(p => p.CourseId);
               

            //modelBuilder.Entity<CourseMember>()
            //    .HasOne(p => p.Account)
            //    .WithMany(c => c.CourseMember)
            //    .HasForeignKey(p => p.AccountId);
                

          


            modelBuilder.Entity<Course>()
              .HasOne(p => p.Account)
              .WithMany(c => c.Courses)
              .HasForeignKey(p => p.AccountId);

            modelBuilder.Entity<CourseContent>()
                .HasOne(p => p.Course)
                .WithMany(c => c.CourseContents)
                .HasForeignKey(p => p.CourseId);

            modelBuilder.Entity<ExerciseContent>()
               .HasOne(p => p.Coursecontent)
               .WithMany(c => c.ExerciseContents)
               .HasForeignKey(p => p.CourseContentId);






            //modelBuilder.Entity<UploadAssignment>()
            // .HasOne(p => p.Account)
            // .WithMany(c => c.UploadAssignment)
            // .HasForeignKey(p => p.AccountId);

            modelBuilder.Entity<UploadAssignment>()
          .HasOne(p => p.ExerciseContent)
          .WithMany(c => c.UploadAssignment)
          .HasForeignKey(p => p.ExerciseContentId)
          .OnDelete(DeleteBehavior.Cascade);






        }


    }
}
