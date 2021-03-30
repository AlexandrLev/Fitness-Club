using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Kursach
{
    public partial class KursuchDBContext : DbContext
    {
        public KursuchDBContext()
        {
        }

        public KursuchDBContext(DbContextOptions<KursuchDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gym> Gyms { get; set; }
        public virtual DbSet<MemberTicket> MemberTickets { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Training> Trainings { get; set; }
        public virtual DbSet<TrainingRegistration> TrainingRegistrations { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=desktop-vqnjpeo\\sqlexpress;Database=KursuchDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Gym>(entity =>
            {
                entity.HasIndex(e => e.GymId, "IX_Gyms_ID");

                entity.Property(e => e.GymId).HasColumnName("GymID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(35);
            });

            modelBuilder.Entity<MemberTicket>(entity =>
            {
                entity.HasIndex(e => e.MemberTicketId, "IX_MemberTickets_ID");

                entity.Property(e => e.MemberTicketId).HasColumnName("MemberTicketID");

                entity.Property(e => e.Cost).HasDefaultValueSql("((0))");

                entity.Property(e => e.GymId).HasColumnName("GymID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(35);

                entity.HasOne(d => d.Gym)
                    .WithMany(p => p.MemberTickets)
                    .HasForeignKey(d => d.GymId)
                    .HasConstraintName("FK_MemberTickets_GymID");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_Coaches_ID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(35);
            });

            modelBuilder.Entity<Training>(entity =>
            {
                entity.HasIndex(e => e.CoachId, "IX_Trainings_CoachID");

                entity.HasIndex(e => e.TrainingId, "IX_Trainings_ID");

                entity.Property(e => e.TrainingId).HasColumnName("TrainingID");

                entity.Property(e => e.CoachId).HasColumnName("CoachID");

                entity.Property(e => e.GymId).HasColumnName("GymID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(35);

                entity.Property(e => e.TimeOfStarting).HasColumnType("datetime");

                entity.HasOne(d => d.Coach)
                    .WithMany(p => p.training)
                    .HasForeignKey(d => d.CoachId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trainings_CoachID");

                entity.HasOne(d => d.Gym)
                    .WithMany(p => p.training)
                    .HasForeignKey(d => d.GymId)
                    .HasConstraintName("FK_Trainings_GymID");
            });

            modelBuilder.Entity<TrainingRegistration>(entity =>
            {
                entity.HasKey(e => new { e.TrainingId, e.ClientId });

                entity.HasIndex(e => e.ClientId, "IX_TrainingRegistrations_ClientID");

                entity.HasIndex(e => e.TrainingId, "IX_TrainingRegistrations_TrainingID");

                entity.Property(e => e.TrainingId).HasColumnName("TrainingID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.TrainingRegistrations)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_TrainingRegistrations_ClientID");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.TrainingRegistrations)
                    .HasForeignKey(d => d.TrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainingRegistrations_TrainingID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_Clients_ID");

                entity.HasIndex(e => e.Login, "UQ__Users__5E55825B8DF3A1E2")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.ConclusionDate).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(35);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(35);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(35);

                entity.Property(e => e.MemberTicketId).HasColumnName("MemberTicketID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(35);

                entity.Property(e => e.Patronymic)
                    .IsRequired()
                    .HasMaxLength(35);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.MemberTicket)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.MemberTicketId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Users_MemberTicketID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Users_RoleID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
