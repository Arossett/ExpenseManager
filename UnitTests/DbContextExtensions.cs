using ExpenseManager.Data;
using ExpenseManager.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    public static class DbContextExtensions
    {
        public static void Seed(this ExpenseManagerContext dbContext)
        {
            dbContext.Users.AddRange(new User[]
            {
                new User{FirstName="Anthony",LastName="Stark",Currency="USD"},
                new User{FirstName="Natasha",LastName="Romanova",Currency="RUB"}
            });
            dbContext.Expenses.AddRange(new Expense[]
                {
                new Expense{UserID = 1, Amount=30, Comment="This is a test comment", CreationDate = DateTime.Now,Type=Expense.ExpenseType.Restaurant, Currency="USD"},
                new Expense{UserID = 1, Amount=30, Comment="This is a test comment", CreationDate = DateTime.Now.AddDays(-2),Type=Expense.ExpenseType.Hotel, Currency="USD"},
                new Expense{UserID = 2, Amount=50, Comment="This is a test comment", CreationDate = DateTime.Now,Type=Expense.ExpenseType.Misc, Currency="RUB"},
            });
            dbContext.SaveChanges();
        }
    }
}
