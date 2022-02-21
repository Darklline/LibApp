using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace LibApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());


            if (!context.MembershipTypes.Any())
            {
                context.MembershipTypes.AddRange(
                new MembershipType
                {
                    Id = 1,
                    Name = "Pay as You Go",
                    SignUpFee = 0,
                    DurationInMonths = 0,
                    DiscountRate = 0
                },
                new MembershipType
                {
                    Id = 2,
                    Name = "Monthly",
                    SignUpFee = 30,
                    DurationInMonths = 1,
                    DiscountRate = 10
                },
                new MembershipType
                {
                    Id = 3,
                    Name = "Quaterly",
                    SignUpFee = 90,
                    DurationInMonths = 3,
                    DiscountRate = 15
                },
                new MembershipType
                {
                    Id = 4,
                    Name = "Yearly",
                    SignUpFee = 300,
                    DurationInMonths = 12,
                    DiscountRate = 20
                });
            }

            if (!context.Customers.Any())
            {
                context.Customers.AddRange(
                new Customer
                {
                    Name = "Adam",
                    HasNewsletterSubscribed = false,
                    MembershipTypeId = 1,
                    Birthdate = new DateTime(2000, 1, 1)
                },
                new Customer
                {
                    Name = "Jacek",
                    HasNewsletterSubscribed = true,
                    MembershipTypeId = 2,
                    Birthdate = new DateTime(2001, 1, 1)
                },
                new Customer
                {
                    Name = "Marek",
                    HasNewsletterSubscribed = false,
                    MembershipTypeId = 3,
                    Birthdate = new DateTime(2002, 1, 1)
                }) ;
            }

            if(!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book
                    {
                        Name = "Lord of the rings",
                        AuthorName = "J.R.R. Tolkien",
                        GenreId = 1,
                        ReleaseDate = new DateTime(2000, 10, 10),
                        NumberAvailable = 3,
                        NumberInStock = 3,
                    },
                    new Book
                    {
                        Name = "Eragon",
                        AuthorName = "Christopher Palaoni",
                        GenreId = 3,
                        ReleaseDate = new DateTime(2000, 10, 10),
                        NumberAvailable = 3,
                        NumberInStock = 3,
                    },
                    new Book
                    {
                        Name = "Wesele",
                        AuthorName = "Stanislaw Wyspianski",
                        GenreId = 5,
                        ReleaseDate = new DateTime(2000,10,10),
                        NumberAvailable = 3,
                        NumberInStock = 3,
                    }
                    );
            }

            context.SaveChanges();
        }
    }
}