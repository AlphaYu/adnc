﻿// <auto-generated />
using System;
using Adnc.Infra.Repository.EfCore.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Adnc.Usr.Repository.Migrations
{
    [DbContext(typeof(MySqlDbContext))]
    partial class MySqlDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4 ");

            modelBuilder.Entity("Adnc.Shared.Repository.EfEntities.EventTracker", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasColumnOrder(1)
                        .HasComment("");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint")
                        .HasColumnName("createby")
                        .HasComment("创建人");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("createtime")
                        .HasComment("创建时间/注册时间");

                    b.Property<long>("EventId")
                        .HasColumnType("bigint")
                        .HasColumnName("eventid")
                        .HasComment("");

                    b.Property<string>("TrackerName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("trackername")
                        .HasComment("");

                    b.HasKey("Id")
                        .HasName("pk_sys_eventtracker");

                    b.HasIndex(new[] { "EventId", "TrackerName" }, "uk_eventid_trackername")
                        .IsUnique()
                        .HasDatabaseName("ix_sys_eventtracker_eventid_trackername");

                    b.ToTable("sys_eventtracker", (string)null);

                    b.HasComment("事件跟踪/处理信息");
                });

            modelBuilder.Entity("Adnc.Usr.Entities.Menu", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasColumnOrder(1)
                        .HasComment("");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("code")
                        .HasComment("编号");

                    b.Property<string>("Component")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("component")
                        .HasComment("組件配置");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint")
                        .HasColumnName("createby")
                        .HasComment("创建人");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("createtime")
                        .HasComment("创建时间/注册时间");

                    b.Property<bool>("Hidden")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("hidden")
                        .HasComment("是否隐藏");

                    b.Property<string>("Icon")
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("icon")
                        .HasComment("图标");

                    b.Property<bool>("IsMenu")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("ismenu")
                        .HasComment("是否是菜单1:菜单,0:按钮");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("isopen")
                        .HasComment("是否默认打开1:是,0:否");

                    b.Property<int>("Levels")
                        .HasColumnType("int")
                        .HasColumnName("levels")
                        .HasComment("级别");

                    b.Property<long?>("ModifyBy")
                        .HasColumnType("bigint")
                        .HasColumnName("modifyby")
                        .HasComment("最后更新人");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("modifytime")
                        .HasComment("最后更新时间");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("name")
                        .HasComment("名称");

                    b.Property<int>("Ordinal")
                        .HasColumnType("int")
                        .HasColumnName("ordinal")
                        .HasComment("序号");

                    b.Property<string>("PCode")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("pcode")
                        .HasComment("父菜单编号");

                    b.Property<string>("PCodes")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasColumnName("pcodes")
                        .HasComment("递归父级菜单编号");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("status")
                        .HasComment("状态1:启用,0:禁用");

                    b.Property<string>("Tips")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("tips")
                        .HasComment("鼠标悬停提示信息");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("url")
                        .HasComment("链接");

                    b.HasKey("Id")
                        .HasName("pk_sys_menu");

                    b.ToTable("sys_menu", (string)null);

                    b.HasComment("菜单");
                });

            modelBuilder.Entity("Adnc.Usr.Entities.Organization", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasColumnOrder(1)
                        .HasComment("");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint")
                        .HasColumnName("createby")
                        .HasComment("创建人");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("createtime")
                        .HasComment("创建时间/注册时间");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("fullname")
                        .HasComment("");

                    b.Property<long?>("ModifyBy")
                        .HasColumnType("bigint")
                        .HasColumnName("modifyby")
                        .HasComment("最后更新人");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("modifytime")
                        .HasComment("最后更新时间");

                    b.Property<int>("Ordinal")
                        .HasColumnType("int")
                        .HasColumnName("ordinal")
                        .HasComment("");

                    b.Property<long?>("Pid")
                        .HasColumnType("bigint")
                        .HasColumnName("pid")
                        .HasComment("");

                    b.Property<string>("Pids")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("varchar(80)")
                        .HasColumnName("pids")
                        .HasComment("");

                    b.Property<string>("SimpleName")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("simplename")
                        .HasComment("");

                    b.Property<string>("Tips")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("tips")
                        .HasComment("");

                    b.HasKey("Id")
                        .HasName("pk_sys_organization");

                    b.ToTable("sys_organization", (string)null);

                    b.HasComment("部门");
                });

            modelBuilder.Entity("Adnc.Usr.Entities.Role", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasColumnOrder(1)
                        .HasComment("");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint")
                        .HasColumnName("createby")
                        .HasComment("创建人");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("createtime")
                        .HasComment("创建时间/注册时间");

                    b.Property<long?>("DeptId")
                        .HasColumnType("bigint")
                        .HasColumnName("deptid")
                        .HasComment("");

                    b.Property<long?>("ModifyBy")
                        .HasColumnType("bigint")
                        .HasColumnName("modifyby")
                        .HasComment("最后更新人");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("modifytime")
                        .HasComment("最后更新时间");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("name")
                        .HasComment("");

                    b.Property<int>("Ordinal")
                        .HasColumnType("int")
                        .HasColumnName("ordinal")
                        .HasComment("");

                    b.Property<long?>("Pid")
                        .HasColumnType("bigint")
                        .HasColumnName("pid")
                        .HasComment("");

                    b.Property<string>("Tips")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("tips")
                        .HasComment("");

                    b.HasKey("Id")
                        .HasName("pk_sys_role");

                    b.ToTable("sys_role", (string)null);

                    b.HasComment("角色");
                });

            modelBuilder.Entity("Adnc.Usr.Entities.RoleRelation", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasColumnOrder(1)
                        .HasComment("");

                    b.Property<long>("MenuId")
                        .HasColumnType("bigint")
                        .HasColumnName("menuid")
                        .HasComment("");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint")
                        .HasColumnName("roleid")
                        .HasComment("");

                    b.HasKey("Id")
                        .HasName("pk_sys_rolerelation");

                    b.HasIndex("MenuId")
                        .HasDatabaseName("ix_sys_rolerelation_menuid");

                    b.ToTable("sys_rolerelation", (string)null);

                    b.HasComment("菜单角色关系");
                });

            modelBuilder.Entity("Adnc.Usr.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasColumnOrder(1)
                        .HasComment("");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("account")
                        .HasComment("账号");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("avatar")
                        .HasComment("头像路径");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("birthday")
                        .HasComment("生日");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint")
                        .HasColumnName("createby")
                        .HasComment("创建人");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("createtime")
                        .HasComment("创建时间/注册时间");

                    b.Property<long?>("DeptId")
                        .HasColumnType("bigint")
                        .HasColumnName("deptid")
                        .HasComment("部门Id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("email")
                        .HasComment("email");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted")
                        .HasColumnOrder(2)
                        .HasComment("");

                    b.Property<long?>("ModifyBy")
                        .HasColumnType("bigint")
                        .HasColumnName("modifyby")
                        .HasComment("最后更新人");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("modifytime")
                        .HasComment("最后更新时间");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("name")
                        .HasComment("姓名");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("password")
                        .HasComment("密码");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)")
                        .HasColumnName("phone")
                        .HasComment("手机号");

                    b.Property<string>("RoleIds")
                        .IsRequired()
                        .HasMaxLength(72)
                        .HasColumnType("varchar(72)")
                        .HasColumnName("roleids")
                        .HasComment("角色id列表，以逗号分隔");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)")
                        .HasColumnName("salt")
                        .HasComment("密码盐");

                    b.Property<int>("Sex")
                        .HasColumnType("int")
                        .HasColumnName("sex")
                        .HasComment("性别");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status")
                        .HasComment("状态");

                    b.HasKey("Id")
                        .HasName("pk_sys_user");

                    b.HasIndex("DeptId")
                        .HasDatabaseName("ix_sys_user_deptid");

                    b.ToTable("sys_user", (string)null);

                    b.HasComment("管理员");
                });

            modelBuilder.Entity("Adnc.Usr.Entities.RoleRelation", b =>
                {
                    b.HasOne("Adnc.Usr.Entities.Menu", "Menu")
                        .WithMany()
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sys_rolerelation_sys_menu_menuid");

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("Adnc.Usr.Entities.User", b =>
                {
                    b.HasOne("Adnc.Usr.Entities.Organization", "Dept")
                        .WithMany()
                        .HasForeignKey("DeptId")
                        .HasConstraintName("fk_sys_user_sys_organization_deptid");

                    b.Navigation("Dept");
                });
#pragma warning restore 612, 618
        }
    }
}
