﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentMenagement.Infrastructure;

namespace StudentMenagement.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210531121946_altherStuden")]
    partial class altherStuden
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StudentMenagement.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaJob")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Studnets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "@ww.com",
                            MaJob = 1,
                            Name = "张三"
                        },
                        new
                        {
                            Id = 2,
                            Email = "@lisi.com",
                            MaJob = 1,
                            Name = "历史"
                        },
                        new
                        {
                            Id = 3,
                            Email = "@zhaoliu.com",
                            MaJob = 1,
                            Name = "赵六"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
