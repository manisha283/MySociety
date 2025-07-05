using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MySociety.Entity.Models;

namespace MySociety.Entity.Data;

public partial class MySocietyDbContext : DbContext
{
    public MySocietyDbContext()
    {
    }

    public MySocietyDbContext(DbContextOptions<MySocietyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AudienceGroup> AudienceGroups { get; set; }

    public virtual DbSet<AudienceGroupMember> AudienceGroupMembers { get; set; }

    public virtual DbSet<AudienceGroupType> AudienceGroupTypes { get; set; }

    public virtual DbSet<Block> Blocks { get; set; }

    public virtual DbSet<Floor> Floors { get; set; }

    public virtual DbSet<House> Houses { get; set; }

    public virtual DbSet<HouseMapping> HouseMappings { get; set; }

    public virtual DbSet<Notice> Notices { get; set; }

    public virtual DbSet<NoticeAttachment> NoticeAttachments { get; set; }

    public virtual DbSet<NoticeAudienceMapping> NoticeAudienceMappings { get; set; }

    public virtual DbSet<NoticeCategory> NoticeCategories { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationCategory> NotificationCategories { get; set; }

    public virtual DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserOtp> UserOtps { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleType> VehicleTypes { get; set; }

    public virtual DbSet<VisitPurpose> VisitPurposes { get; set; }

    public virtual DbSet<Visitor> Visitors { get; set; }

    public virtual DbSet<VisitorFeedback> VisitorFeedbacks { get; set; }

    public virtual DbSet<VisitorStatus> VisitorStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DbConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AudienceGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AudienceGroups_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.GroupName)
                .HasMaxLength(100)
                .HasColumnName("group_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AudienceGroupCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AudienceGroups_created_by_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AudienceGroupUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AudienceGroups_updated_by_fkey");
        });

        modelBuilder.Entity<AudienceGroupMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AudienceGroupMembers_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AudienceGroupId).HasColumnName("audience_group_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.MemberId).HasColumnName("member_id");

            entity.HasOne(d => d.AudienceGroup).WithMany(p => p.AudienceGroupMembers)
                .HasForeignKey(d => d.AudienceGroupId)
                .HasConstraintName("AudienceGroupMembers_audience_group_id_fkey");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AudienceGroupMemberCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AudienceGroupMembers_created_by_fkey");

            entity.HasOne(d => d.Member).WithMany(p => p.AudienceGroupMemberMembers)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("AudienceGroupMembers_member_id_fkey");
        });

        modelBuilder.Entity<AudienceGroupType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AudienceGroupTypes_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Block>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Blocks_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BlockNumber).HasColumnName("block_number");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NoOfFloor).HasColumnName("no_of_floor");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BlockCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Blocks_created_by_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BlockUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Blocks_updated_by_fkey");
        });

        modelBuilder.Entity<Floor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Floors_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.FloorNumber).HasColumnName("floor_number");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NoOfHouse).HasColumnName("no_of_house");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FloorCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Floors_created_by_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FloorUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Floors_updated_by_fkey");
        });

        modelBuilder.Entity<House>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Houses_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.HouseNumber).HasColumnName("house_number");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HouseCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Houses_created_by_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.HouseUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Houses_updated_by_fkey");
        });

        modelBuilder.Entity<HouseMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HouseMapping_pkey");

            entity.ToTable("HouseMapping");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BlockId).HasColumnName("block_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.FloorId).HasColumnName("floor_id");
            entity.Property(e => e.HouseId).HasColumnName("house_id");
            entity.Property(e => e.HouseName)
                .HasMaxLength(15)
                .HasColumnName("house_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Block).WithMany(p => p.HouseMappings)
                .HasForeignKey(d => d.BlockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HouseMapping_block_id_fkey");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HouseMappingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HouseMapping_created_by_fkey");

            entity.HasOne(d => d.Floor).WithMany(p => p.HouseMappings)
                .HasForeignKey(d => d.FloorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HouseMapping_floor_id_fkey");

            entity.HasOne(d => d.House).WithMany(p => p.HouseMappings)
                .HasForeignKey(d => d.HouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HouseMapping_house_id_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.HouseMappingUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HouseMapping_updated_by_fkey");
        });

        modelBuilder.Entity<Notice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Notices_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.NoticeCategoryId).HasColumnName("notice_category_id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NoticeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Notices_created_by_fkey");

            entity.HasOne(d => d.NoticeCategory).WithMany(p => p.Notices)
                .HasForeignKey(d => d.NoticeCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Notices_notice_category_id_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.NoticeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Notices_updated_by_fkey");
        });

        modelBuilder.Entity<NoticeAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NoticeAttachments_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.NoticeId).HasColumnName("notice_id");
            entity.Property(e => e.Path).HasColumnName("path");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NoticeAttachments)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("NoticeAttachments_created_by_fkey");

            entity.HasOne(d => d.Notice).WithMany(p => p.NoticeAttachments)
                .HasForeignKey(d => d.NoticeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("NoticeAttachments_notice_id_fkey");
        });

        modelBuilder.Entity<NoticeAudienceMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NoticeAudienceMapping_pkey");

            entity.ToTable("NoticeAudienceMapping");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.GroupTypeId).HasColumnName("group_type_id");
            entity.Property(e => e.NoticeId).HasColumnName("notice_id");
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NoticeAudienceMappings)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("NoticeAudienceMapping_created_by_fkey");

            entity.HasOne(d => d.GroupType).WithMany(p => p.NoticeAudienceMappings)
                .HasForeignKey(d => d.GroupTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("NoticeAudienceMapping_group_type_id_fkey");

            entity.HasOne(d => d.Notice).WithMany(p => p.NoticeAudienceMappings)
                .HasForeignKey(d => d.NoticeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("NoticeAudienceMapping_notice_id_fkey");
        });

        modelBuilder.Entity<NoticeCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NoticeCategories_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Notifications_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .HasColumnName("message");
            entity.Property(e => e.ReadAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("read_at");
            entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.TargetEntity)
                .HasMaxLength(100)
                .HasColumnName("target_entity");
            entity.Property(e => e.TargetEntityId).HasColumnName("target_entity_id");
            entity.Property(e => e.TargetId).HasColumnName("target_id");
            entity.Property(e => e.TargetUrl)
                .HasMaxLength(500)
                .HasColumnName("target_url");

            entity.HasOne(d => d.Receiver).WithMany(p => p.NotificationReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Notifications_receiver_id_fkey");

            entity.HasOne(d => d.Sender).WithMany(p => p.NotificationSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("Notifications_sender_id_fkey");
        });

        modelBuilder.Entity<NotificationCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NotificationCategories_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ResetPasswordToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ResetPasswordToken_pkey");

            entity.ToTable("ResetPasswordToken");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Expirytime)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP + '24:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expirytime");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.Token)
                .HasColumnType("character varying")
                .HasColumnName("token");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Roles_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.HouseUnitId).HasColumnName("house_unit_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.IsApproved).HasColumnName("is_approved");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.ProfileImg)
                .HasColumnType("character varying")
                .HasColumnName("profile_img");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.HouseUnit).WithMany(p => p.Users)
                .HasForeignKey(d => d.HouseUnitId)
                .HasConstraintName("Users_house_unit_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_role_id_fkey");
        });

        modelBuilder.Entity<UserOtp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserOtp_pkey");

            entity.ToTable("UserOtp");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExpiryTime)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP + '00:10:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expiry_time");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.OtpCode)
                .HasMaxLength(8)
                .HasColumnName("otp_code");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserOtps)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserOtp_user_id_fkey");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Vehicles_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.ParkingSlotNo).HasColumnName("parking_slot_no");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VehicleNumber)
                .HasMaxLength(15)
                .HasColumnName("vehicle_number");
            entity.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VehicleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_created_by_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.VehicleUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_updated_by_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.VehicleUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_user_id_fkey");

            entity.HasOne(d => d.VehicleType).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.VehicleTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_vehicle_type_id_fkey");
        });

        modelBuilder.Entity<VehicleType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VehicleType_pkey");

            entity.ToTable("VehicleType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<VisitPurpose>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VisitPurpose_pkey");

            entity.ToTable("VisitPurpose");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Visitors_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CheckInTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("check_in_time");
            entity.Property(e => e.CheckOutTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("check_out_time");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.HouseMappingId).HasColumnName("house_mapping_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NoOfVisitors).HasColumnName("no_of_visitors");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VehicleNo)
                .HasMaxLength(15)
                .HasColumnName("vehicle_no");
            entity.Property(e => e.VisitPurposeId).HasColumnName("visit_purpose_id");
            entity.Property(e => e.VisitPurposeReason)
                .HasColumnType("character varying")
                .HasColumnName("visit_purpose_reason");

            entity.HasOne(d => d.HouseMapping).WithMany(p => p.Visitors)
                .HasForeignKey(d => d.HouseMappingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Visitors_house_mapping_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Visitors)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Visitors_status_id_fkey");

            entity.HasOne(d => d.VisitPurpose).WithMany(p => p.Visitors)
                .HasForeignKey(d => d.VisitPurposeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Visitors_visit_purpose_id_fkey");
        });

        modelBuilder.Entity<VisitorFeedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VisitorFeedbacks_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Feedback)
                .HasMaxLength(500)
                .HasColumnName("feedback");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.VisitorId).HasColumnName("visitor_id");

            entity.HasOne(d => d.Visitor).WithMany(p => p.VisitorFeedbacks)
                .HasForeignKey(d => d.VisitorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VisitorFeedbacks_visitor_id_fkey");
        });

        modelBuilder.Entity<VisitorStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VisitorStatus_pkey");

            entity.ToTable("VisitorStatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
