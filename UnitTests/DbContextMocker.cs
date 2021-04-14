using ExpenseManager.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    public static class DbContextMocker
    {
        public static ExpenseManagerContext GetExpenseManagerDbContext(string dbName)
        {
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<ExpenseManagerContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            // Create instance of DbContext
            var dbContext = new ExpenseManagerContext(options);

            // Add entities in memory
            dbContext.Seed();

            return dbContext;
        }
    }
}
