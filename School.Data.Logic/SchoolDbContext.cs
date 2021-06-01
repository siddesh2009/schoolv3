using Microsoft.EntityFrameworkCore;
using School.Data.Model;
using SchoolModel.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Data.Logic
{
    public partial class SchoolDbContext : DbContext
    {
        public SchoolDbContext()
        { }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
    : base(options)
        { }

        public virtual DbSet<boardgradesubassoc> boardgradesubassocs { get; set; }
        public virtual DbSet<boardmaster> boardmasters { get; set; }
        public virtual DbSet<citiesmaster> citiesmasters { get; set; }
        public virtual DbSet<countrymaster> countrymasters { get; set; }
        public virtual DbSet<grademaster> grademasters { get; set; }
        public virtual DbSet<menumaster> menumasters { get; set; }
        public virtual DbSet<menuroleassoc> menuroleassocs { get; set; }
        public virtual DbSet<refreshtoken> refreshtokens { get; set; }
        public virtual DbSet<rolemaster> rolemasters { get; set; }
        public virtual DbSet<schooluser> schoolusers { get; set; }
        public virtual DbSet<statesmaster> statesmasters { get; set; }
        public virtual DbSet<subjectmaster> subjectmasters { get; set; }
        public virtual DbSet<userdetail> userdetails { get; set; }
        public virtual DbSet<userroleassoc> userroleassocs { get; set; }
        public virtual DbSet<userroleschedule> userroleschedule { get; set; }
        public virtual DbSet<errorLogger> errorLogger { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
