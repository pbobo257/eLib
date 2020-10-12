using eLib.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace eLib.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        DbSet<Account> Accounts { get; set; }
        DbSet<BookHeader> BooksHeaders { get; set; }
        DbSet<BookDetails> BooksDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>().HasKey(x => x.Id);
            builder.Entity<Account>().Property(x => x.UserType).HasConversion<string>().IsRequired();
            builder.Entity<Account>().Property(x => x.Login).IsRequired();
            builder.Entity<Account>().Property(x => x.Password).IsRequired();
            builder.Entity<Account>().Property(x => x.Email).IsRequired();

            builder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    Login = "DefaultAdmin",
                    Password = "DefaultAdminPassword123",
                    Email = "DefaultAdmin@gmail.com",
                    UserType = UserType.Admin
                },
                new Account
                {
                    Id = 2,
                    Login = "DefaultUser",
                    Password = "DefaultUserPassword123",
                    Email = "DefaultUser@gmail.com",
                    UserType = UserType.User
                });

            builder.Entity<BookHeader>().HasKey(x => x.Id);
            builder.Entity<BookHeader>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Entity<BookHeader>().Property(x => x.Cover).IsRequired();

            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(@"E:\Study\3_курс_1_семестр\kpz\sampleImage.png", FileMode.Open, FileAccess.Read))
            {
                file.CopyTo(ms);
            }
            builder.Entity<BookHeader>().HasData(
                new BookHeader
                {
                    Id = 1,
                    Name = "Дизайн Паттерни - просто, як двері",
                    Cover = ms.ToArray()
                }
            );
            ms.Close();
            ms.Dispose();

            builder.Entity<BookDetails>().HasKey(x => x.Id);
            builder.Entity<BookDetails>().Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Entity<BookDetails>().Property(x => x.Book).IsRequired();
            builder.Entity<BookDetails>().Property(x => x.ReleaseDate).HasColumnType("date").IsRequired();
            builder.Entity<BookDetails>().Property(x => x.Genre).IsRequired().HasMaxLength(50);

            builder.Entity<BookDetails>()
                .HasOne(x => x.Header)
                .WithOne(x => x.Details)
                .HasForeignKey<BookDetails>(x => x.HeaderId);
        }
    }
}
