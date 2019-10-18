﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PVT.Money.Data;
using System;

namespace PVT.Money.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20180323062546_AddAccounts")]
    partial class AddAccounts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PVT.Money.Data.AccountEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<decimal>("Amount")
                        .HasColumnName("Amount");

                    b.Property<string>("Currency")
                        .HasColumnName("Currency");

                    b.Property<bool>("IsCommission")
                        .HasColumnName("IsCommission");

                    b.Property<int>("UserID")
                        .HasColumnName("UserId");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("PVT.Money.Data.LogEventEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnName("Event");

                    b.Property<int>("UserID")
                        .HasColumnName("UserId");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("LogEvents");
                });

            modelBuilder.Entity("PVT.Money.Data.PermissionEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Permission")
                        .IsRequired()
                        .HasColumnName("Permission");

                    b.HasKey("ID");

                    b.HasIndex("Permission")
                        .IsUnique();

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("PVT.Money.Data.PermissionToRoleEntity", b =>
                {
                    b.Property<int>("RoleID")
                        .HasColumnName("RoleId");

                    b.Property<int>("PermissionID")
                        .HasColumnName("PermissionId");

                    b.HasKey("RoleID", "PermissionID");

                    b.HasIndex("PermissionID");

                    b.ToTable("PermissionsToRoles");
                });

            modelBuilder.Entity("PVT.Money.Data.UserEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("Email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("Password");

                    b.Property<int>("RoleID")
                        .HasColumnName("RoleId");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnName("UserName");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleID");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PVT.Money.Data.UserInfoEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Address")
                        .HasColumnName("Address");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnName("BirthDate");

                    b.Property<string>("FirstName")
                        .HasColumnName("FirstName");

                    b.Property<string>("Gender")
                        .HasColumnName("Gender");

                    b.Property<string>("LastName")
                        .HasColumnName("LastName");

                    b.Property<string>("Phone")
                        .HasColumnName("Phone");

                    b.Property<int>("UserID")
                        .HasColumnName("UserId");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("UsersInfo");
                });

            modelBuilder.Entity("PVT.Money.Data.UserRoleEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnName("Role");

                    b.HasKey("ID");

                    b.HasIndex("Role")
                        .IsUnique();

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("PVT.Money.Data.AccountEntity", b =>
                {
                    b.HasOne("PVT.Money.Data.UserEntity", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PVT.Money.Data.LogEventEntity", b =>
                {
                    b.HasOne("PVT.Money.Data.UserEntity", "User")
                        .WithMany("LogEvents")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PVT.Money.Data.PermissionToRoleEntity", b =>
                {
                    b.HasOne("PVT.Money.Data.PermissionEntity", "Permission")
                        .WithMany("PermissionRoles")
                        .HasForeignKey("PermissionID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PVT.Money.Data.UserRoleEntity", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PVT.Money.Data.UserEntity", b =>
                {
                    b.HasOne("PVT.Money.Data.UserRoleEntity", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PVT.Money.Data.UserInfoEntity", b =>
                {
                    b.HasOne("PVT.Money.Data.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}