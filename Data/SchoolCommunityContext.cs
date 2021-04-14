using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab4.Models;
using Lab4.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Data
{
    public class SchoolCommunityContext : DbContext
    {
        public SchoolCommunityContext(DbContextOptions<SchoolCommunityContext> options) : base(options)
        {
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<CommunityMembership> CommunityMembership { get; set; }
        public DbSet<Community> Community { get; set; }
        //public DbSet<CommunityViewModel> Communities { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Community>().ToTable("Community");
            modelBuilder.Entity<CommunityMembership>().ToTable("CommunityMembership");

            modelBuilder.Entity<CommunityMembership>()
                    .HasKey(c => new
                    {
                        c.StudentID,
                        c.CommunityID
                    });      
        }
    }
}
