using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Domain.Entities.Procedure;

namespace SiaAdmin.Persistence.Contexts
{
    public partial class SiaAdminDbContext : DbContext
    {
        public SiaAdminDbContext(DbContextOptions<SiaAdminDbContext> options) : base(options: options)
        {
        }

        public virtual DbSet<BlockList> BlockLists { get; set; }

        public virtual DbSet<Incentive> Incentives { get; set; }
        public virtual DbSet<SiaUser> SiaUsers { get; set; }

        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<SiaRole> SiaRoles { get; set; }
        public virtual DbSet<SiaUserRole> SiaUserRoles { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<EODPTable> Eodptables { get; set; }
        public virtual DbSet<OtpHistory> Otphistories { get; set; }
        public virtual DbSet<Scm> Scms { get; set; }
        public virtual DbSet<FilterData> FilterData { get; set; }
        public virtual DbSet<SurveyAssigned> SurveysAssigneds { get; set; } 
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WaitData> WaitData { get; set; }
        public virtual DbSet<SurveyLog> SurveyLogs { get; set; }
        public virtual DbSet<EODTable> Eodtables { get; set; }


        #region Stored Procedure

        public virtual DbSet<ToplamAnketBilgisi> ToplamAnketBilgisi { get; set; }
        public virtual DbSet<PanelistSaatKullanimi> PanelistSaatKullanimi { get; set; }

        public virtual DbSet<TanitimAnketiDolduran> TanitimAnketiDolduran { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlockList>(entity =>
            {
                entity.ToTable("BlockList");

                entity.Property(e => e.Data).HasMaxLength(50);
                entity.Property(e => e.Note).HasMaxLength(100);
                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });
            modelBuilder.Entity<SurveyLog>(entity =>
            {
                entity.ToTable("SurveyLog");

                entity.Property(e => e.SurveyUserGuid).HasColumnName("SurveyUserGUID");
                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });
            modelBuilder.Entity<Incentive>(entity =>
            {
                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });
            modelBuilder.Entity<FilterData>(entity =>
            {
                entity.Property(e => e.Acikyas).HasColumnName("ACIKYAS");
                entity.Property(e => e.AmeslekGk).HasColumnName("AMeslekGK");
                entity.Property(e => e.AmeslekHr).HasColumnName("AMeslekHR");
                entity.Property(e => e.Bolge).HasColumnName("BOLGE");
                entity.Property(e => e.Cinsiyet).HasColumnName("CINSIYET");
                entity.Property(e => e.EgitimGk).HasColumnName("EgitimGK");
                entity.Property(e => e.EgitimHr).HasColumnName("EgitimHR");
                entity.Property(e => e.Grupyas).HasColumnName("GRUPYAS");
                entity.Property(e => e.Hhr).HasColumnName("HHR");
                entity.Property(e => e.Il).HasColumnName("IL");
                entity.Property(e => e.Ilce).HasColumnName("ILCE");
                entity.Property(e => e.SurveyUserGuid).HasColumnName("SurveyUserGUID");
                entity.Property(e => e.YmeslekDetayGk).HasColumnName("YMeslekDetayGK");
                entity.Property(e => e.YmeslekDetayHr).HasColumnName("YMeslekDetayHR");
                entity.Property(e => e.YmeslekGk).HasColumnName("YMeslekGK");
                entity.Property(e => e.YmeslekHr).HasColumnName("YMeslekHR");
                entity.Property(e => e.Yses).HasColumnName("YSES");
            });
            modelBuilder.Entity<SiaRole>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Role");

                entity.ToTable("SiaRole");

                entity.Property(e => e.RoleType).HasMaxLength(50);
            });
            modelBuilder.Entity<SiaUser>(entity =>
            {
                entity.ToTable("SiaUser");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
                entity.Property(e => e.LastFailedLoginDate).HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(150);
                entity.Property(e => e.PhoneNumber).HasMaxLength(50);
                entity.Property(e => e.Surname).HasMaxLength(50);
                entity.Property(e => e.UserGUID).HasColumnName("UserGUID");
                entity.Property(e => e.UserName).HasMaxLength(150);
            });
            modelBuilder.Entity<OtpHistory>(entity =>
            {
                entity.ToTable("OTPHistory");

                entity.Property(e => e.LastIp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastIP");
                entity.Property(e => e.Otp).HasColumnName("OTP");
                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });
            modelBuilder.Entity<Scm>(entity =>
            {
                entity.ToTable("SCM");

                entity.Property(e => e.Scmid)
                    .ValueGeneratedNever()
                    .HasColumnName("SCMId");
                entity.Property(e => e.Scm1).HasColumnName("SCM");
            });
            modelBuilder.Entity<SiaUserRole>(entity =>
            {
                entity.ToTable("SiaUserRole");

                entity.HasOne(d => d.Role).WithMany(p => p.SiaUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_SiaUserRole_SiaRole");

                entity.HasOne(d => d.User).WithMany(p => p.SiaUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SiaUserRole_SiaUser");
            });
            modelBuilder.Entity<Survey>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("SurveyId");
                entity.Property(e => e.DBAddress)
                    .HasMaxLength(250)
                    .HasColumnName("DBAddress");
                entity.Property(e => e.DPPass)
                    .HasMaxLength(250)
                    .HasColumnName("DPPass");
                entity.Property(e => e.DPUser)
                    .HasMaxLength(250)
                    .HasColumnName("DPUser");
                entity.Property(e => e.SurveyDescription).HasMaxLength(250);
                entity.Property(e => e.SurveyLink).HasMaxLength(250);
                entity.Property(e => e.SurveyLinkText).HasMaxLength(250);
                entity.Property(e => e.SurveyRedirect).HasMaxLength(250);
                entity.Property(e => e.SurveyStartDate).HasColumnType("datetime");
                entity.Property(e => e.SurveyText).HasMaxLength(250);
                entity.Property(e => e.SurveyValidity).HasColumnType("datetime");
                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });
            modelBuilder.Entity<SurveyAssigned>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_SurveyAssigneds");

                entity.ToTable("SurveysAssigned");

                entity.Property(e => e.InternalGuid).HasColumnName("InternalGUID");
                entity.Property(e => e.SurveyDescription).HasMaxLength(250);
                entity.Property(e => e.SurveyLink).HasMaxLength(250);
                entity.Property(e => e.SurveyLinkText).HasMaxLength(250);
                entity.Property(e => e.SurveyRedirect).HasMaxLength(50);
                entity.Property(e => e.SurveyStartDate).HasColumnType("datetime");
                entity.Property(e => e.SurveyText).HasMaxLength(250);
                entity.Property(e => e.SurveyValidity).HasColumnType("datetime");
                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false);
                entity.Property(e => e.InternalGuid).HasColumnName("InternalGUID");
                entity.Property(e => e.Iys)
                    .HasColumnType("datetime")
                    .HasColumnName("IYS");
                entity.Property(e => e.LastFailedLogin).HasColumnType("datetime");
                entity.Property(e => e.LastIp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastIP");
                entity.Property(e => e.LastLogin).HasColumnType("datetime");
                entity.Property(e => e.Msisdn)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.MyReference)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.ReferredBy)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.RegionCode)
                    .HasMaxLength(4)
                    .IsUnicode(false);
                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
                entity.Property(e => e.Scmtarihi)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SCMTarihi");
                entity.Property(e => e.Surname).HasMaxLength(100);
                entity.Property(e => e.SurveyUserGuid).HasColumnName("SurveyUserGUID");
                entity.Property(e => e.Tckno)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("TCKNo");
            });
            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.Description).HasColumnType("text");
            });
            modelBuilder.Entity<WaitData>(entity =>
            {
                entity.Property(e => e.SurveyUserGuid).HasColumnName("SurveyUserGUID");
                entity.Property(e => e.Tarih).HasColumnType("datetime");
                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            }); 
            modelBuilder.Entity<EODTable>(entity =>
            {
                entity.ToTable("EODTable");

                entity.Property(e => e.SurveyUserGuid).HasColumnName("SurveyUserGUID");
                entity.Property(e => e.Timestamp).HasColumnType("datetime");
                entity.Property(e => e.ToplamPara).HasColumnType("decimal(19, 4)");
                entity.Property(e => e.ToplamPuan).HasColumnType("decimal(19, 0)");
            });
            modelBuilder.Entity<EODPTable>(entity =>
            {
                entity.ToTable("EODPTable");

                entity.Property(e => e.SurveyDbaddress)
                    .HasMaxLength(50)
                    .HasColumnName("SurveyDBAddress");
                entity.Property(e => e.SurveyDescription).HasMaxLength(200);
                entity.Property(e => e.SurveyStartDate).HasColumnType("datetime");
                entity.Property(e => e.SurveyStatus).HasMaxLength(5);
                entity.Property(e => e.SurveyText).HasMaxLength(50);
                entity.Property(e => e.SurveyValidity).HasColumnType("datetime");
                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });
            modelBuilder.Entity<ToplamAnketBilgisi>().HasNoKey();
            modelBuilder.Entity<TanitimAnketiDolduran>().HasNoKey();
            modelBuilder.Entity<PanelistSaatKullanimi>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.Pazartesi).HasColumnName("1");
                entity.Property(e => e.Sali).HasColumnName("2");
                entity.Property(e => e.Carsamba).HasColumnName("3");
                entity.Property(e => e.Persembe).HasColumnName("4");
                entity.Property(e => e.Cuma).HasColumnName("5");
                entity.Property(e => e.Cumartesi).HasColumnName("6");
                entity.Property(e => e.Pazar).HasColumnName("7");

            });
            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

