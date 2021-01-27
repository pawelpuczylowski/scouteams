using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScouTeams.Areas.Identity.Data;
using ScouTeams.Models;

namespace ScouTeams.Data
{
    public class ScouTDBContext : IdentityDbContext<Scout>
    {
        public ScouTDBContext(DbContextOptions<ScouTDBContext> options)
            : base(options)
        {
        }

        public DbSet<Choragiew> Choragiews { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Druzyna> Druzynas { get; set; }
        public DbSet<FunctionInOrganization> FunctionInOrganizations { get; set; }
        public DbSet<Hufiec> Hufiecs { get; set; }
        public DbSet<KwateraGlowna> KwateraGlowna { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Scout> Scouts { get; set; }
        public DbSet<ScoutPresence> ScoutPresence { get; set; }
        public DbSet<Zastep> Zastep { get; set; }
        public DbSet<UserZastep> UserZasteps { get; set; }
        public DbSet<UserDruzyna> UserDruzynas { get; set; }
        public DbSet<UserHufiec> UserHufiecs { get; set; }
        public DbSet<UserChoragiew> UserChoragiews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            modelBuilder.Entity<Scout>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Zastep>()
                .HasKey(x => x.ZastepId);

            modelBuilder.Entity<UserZastep>()
                .HasKey(x => new { x.ScoutId, x.ZastepId });
            modelBuilder.Entity<UserZastep>()
                .HasOne(x => x.MyScout)
                .WithMany(z => z.Zasteps)
                .HasForeignKey(x => x.ScoutId);
            modelBuilder.Entity<UserZastep>()
                .HasOne(x => x.Zastep)
                .WithMany(s => s.Scouts)
                .HasForeignKey(x => x.ZastepId);


            modelBuilder.Entity<Hufiec>()
                .HasKey(x => x.HufiecId);

            modelBuilder.Entity<UserHufiec>()
                .HasKey(x => new { x.ScoutId, x.HufiecId });
            modelBuilder.Entity<UserHufiec>()
                .HasOne(x => x.MyScout)
                .WithMany(h => h.Hufiecs)
                .HasForeignKey(x => x.ScoutId);
            modelBuilder.Entity<UserHufiec>()
                .HasOne(x => x.Hufiec)
                .WithMany(s => s.Scouts)
                .HasForeignKey(x => x.HufiecId);


            modelBuilder.Entity<Druzyna>()
                .HasKey(x => x.DruzynaId);

            modelBuilder.Entity<UserDruzyna>()
                .HasKey(x => new { x.ScoutId, x.DruzynaId });
            modelBuilder.Entity<UserDruzyna>()
                .HasOne(x => x.MyScout)
                .WithMany(d => d.Druzynas)
                .HasForeignKey(x => x.ScoutId);
            modelBuilder.Entity<UserDruzyna>()
                .HasOne(x => x.Druzyna)
                .WithMany(s => s.Scouts)
                .HasForeignKey(x => x.DruzynaId);


            modelBuilder.Entity<Choragiew>()
                .HasKey(x => x.ChoragiewId);

            modelBuilder.Entity<UserChoragiew>()
                .HasKey(x => new { x.ScoutId, x.ChoragiewId });
            modelBuilder.Entity<UserChoragiew>()
                .HasOne(x => x.MyScout)
                .WithMany(c => c.Choragiews)
                .HasForeignKey(x => x.ScoutId);
            modelBuilder.Entity<UserChoragiew>()
                .HasOne(x => x.Choragiew)
                .WithMany(s => s.Scouts)
                .HasForeignKey(x => x.ChoragiewId);
        }
    }
}
