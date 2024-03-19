﻿using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Flagging.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Flag> Flags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "Flagging");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        private IEnumerable<Comment> CommentSeed()
        {
            for (var i = 1; i <= 25; i++)
            {
                yield return new Comment
                {
                    Id = i,
                    UserId = i,
                    Message = Faker.Lorem.Sentence(5)
                };
            }
        }

        private IEnumerable<Article> ArticleSeed()
        {
            for (var i = 1; i <= 25; i++)
            {
                yield return new Article
                {
                    Id = i,
                    UserId = i,
                    Description = Faker.Lorem.Paragraph(5)
                };
            }
        }

        private IEnumerable<User> UsersSeed()
        {
            for (var i = 1; i <= 25; i++)
            {
                yield return new User
                {
                    Id = i,
                    FirstName = Faker.Name.First(),
                    LastName = Faker.Name.Last()
                };
            }
        }

        private IEnumerable<Flag> FlagSeed()
        {
            var j = 1;

            for (var i = 1; i <= 25; i++)
            {
                for (var z = 0; z < Faker.RandomNumber.Next(5); z++)
                {
                    yield return new Flag
                    {
                        Id = j++,
                        ArticleId = i,
                        Type = FlaggedContentType.Article,
                        DateCreated = Faker.Identification.DateOfBirth(),
                    };
                }
            }

            for (var i = 1; i <= 25; i++)
            {
                for (var z = 0; z < Faker.RandomNumber.Next(5); z++)
                {
                    yield return new Flag
                    {
                        Id = j++,
                        CommentId = i,
                        Type = FlaggedContentType.Comment,
                        DateCreated = Faker.Identification.DateOfBirth(),
                    };
                }
            }

            for (var i = 1; i <= 25; i++)
            {
                for (var z = 0; z < Faker.RandomNumber.Next(5); z++)
                {
                    yield return new Flag
                    {
                        Id = j++,
                        ReportedUserId = i,
                        Type = FlaggedContentType.Member,
                        DateCreated = Faker.Identification.DateOfBirth(),
                    };
                }
            }
        }

        public void Seed()
        {
            foreach (var user in UsersSeed())
            {
                Users.Add(user);
            }

            foreach (var article in ArticleSeed())
            {
                Articles.Add(article);
            }   

            foreach (var comment in CommentSeed())
            {
                Comments.Add(comment);
            }

            foreach (var flag in FlagSeed())
            {
                Flags.Add(flag);
            }
        }
    }
}
