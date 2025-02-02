using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Admins> Admin { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<ShiftType> ShiftType { get; set; }

        public DbSet<Branch> Branch { get; set; }
        public DbSet<Shift> Shift { get; set; }
        public DbSet<Worker> Worker { get; set; }
        public DbSet<Profession> Profession { get; set; }
        public DbSet<WorkerProfession> WorkerProfession { get; set; }
        public DbSet<WorkerShift> WorkerShift { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Admins>(entity =>
    {
        entity.HasKey(a => a.userID);

        entity.HasOne(a => a.User)
              .WithOne(u => u.Admins)
              .HasForeignKey<Admins>(a => a.userID)
              .OnDelete(DeleteBehavior.Cascade);
    });
            base.OnModelCreating(builder);

            builder.Entity<WorkerShift>(x => x.HasKey(p => new { p.ShiftID, p.WorkerID }));

            builder.Entity<WorkerShift>()
            .HasOne(u => u.Shift)
            .WithMany(u => u.WorkerShifts)
            .HasForeignKey(p => p.ShiftID);

            builder.Entity<WorkerShift>()
            .HasOne(u => u.Worker)
            .WithMany(u => u.WorkerShifts)
            .HasForeignKey(p => p.WorkerID);

            builder.Entity<WorkerProfession>(x => x.HasKey(wp => new { wp.workerID, wp.professionID }));

            builder.Entity<WorkerProfession>()
          .HasOne(u => u.Profession)
          .WithMany(u => u.WorkerProfessions)
          .HasForeignKey(p => p.professionID);

            builder.Entity<WorkerProfession>()
            .HasOne(u => u.Worker)
            .WithMany(u => u.WorkerProfessions)
            .HasForeignKey(p => p.workerID);
        }
    }
}

