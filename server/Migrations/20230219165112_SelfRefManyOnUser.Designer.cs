// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TypeRunnerBE.Models;

#nullable disable

namespace TypeRunnerBE.Migrations
{
    [DbContext(typeof(TypeMarathonContext))]
    [Migration("20230219165112_SelfRefManyOnUser")]
    partial class SelfRefManyOnUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TypeMarathonBE.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "asdf",
                            Username = "asdf"
                        },
                        new
                        {
                            Id = 2,
                            Password = "qwer",
                            Username = "qwer"
                        });
                });

            modelBuilder.Entity("TypeMarathonBE.Models.UserFriend", b =>
                {
                    b.Property<int>("FromUserId")
                        .HasColumnType("integer");

                    b.Property<int>("ToUserId")
                        .HasColumnType("integer");

                    b.HasKey("FromUserId", "ToUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("UserFriend");

                    b.HasData(
                        new
                        {
                            FromUserId = 1,
                            ToUserId = 2
                        });
                });

            modelBuilder.Entity("TypeMarathonBE.Models.UserFriend", b =>
                {
                    b.HasOne("TypeMarathonBE.Models.User", "FromUser")
                        .WithMany()
                        .HasForeignKey("FromUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TypeMarathonBE.Models.User", "ToUser")
                        .WithMany()
                        .HasForeignKey("ToUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });
#pragma warning restore 612, 618
        }
    }
}
