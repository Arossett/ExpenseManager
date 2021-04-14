using ExpenseManager.Data;
using ExpenseManager.Models;
using System;
using System.Linq;

namespace ExpenseManager.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ExpenseManagerContext context)
        {
            context.Database.EnsureCreated();

            // Look for any user.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }
            var users = new User[]
            {
                new User{FirstName="Anthony",LastName="Stark",Currency="USD"},
                new User{FirstName="Natasha",LastName="Romanova",Currency="RUB"}
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}