using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Models;

namespace ExpenseManager.Data
{
    public class ExpenseManagerContext : DbContext
    {
        public ExpenseManagerContext (DbContextOptions<ExpenseManagerContext> options)
            : base(options)
        {
        }
        public DbSet<ExpenseManager.Models.User> Users { get; set; }
        public DbSet<ExpenseManager.Models.Expense> Expenses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Expense>().ToTable("Expenses");
        }

    }
}
