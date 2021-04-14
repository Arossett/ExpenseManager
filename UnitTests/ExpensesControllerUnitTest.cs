using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using ExpenseManager.Controllers;
using ExpenseManager.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UnitTests
{
    public class ExpensesControllerUnitTest
    {
        [Fact]
        public async Task TestGetExpensesAsync()
        {
            var dbContext = DbContextMocker.GetExpenseManagerDbContext(nameof(TestGetExpensesAsync));
            var controller = new ExpensesController(dbContext);

            var response = await controller.GetExpense();
            var value = response.Value as List<Expense>;

            dbContext.Dispose();

            Assert.IsType<ActionResult<IEnumerable<Expense>>>(response);
            Assert.Equal(3, value.Count);
        }

        [Fact]
        public async Task TestGetExpensesByUserIdAsync()
        {
            var dbContext = DbContextMocker.GetExpenseManagerDbContext(nameof(TestGetExpensesByUserIdAsync));
            var controller = new ExpensesController(dbContext);

            var response = await controller.GetExpenseByUser(1,"");
            var value = response.Value;

            dbContext.Dispose();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<Expense>>>(response);
            Assert.IsType<List<Expense>>(value);
            Assert.Equal(2, (value as List<Expense>).Count);
        }

        [Fact]
        public async Task TestValidPostExpenseAsync()
        {
            var dbContext = DbContextMocker.GetExpenseManagerDbContext(nameof(TestValidPostExpenseAsync));
            var controller = new ExpensesController(dbContext);

            var expense = new Expense
            {
                UserID = 1,
                Amount = 10,
                Comment = "This is a test comment",
                CreationDate = DateTime.Now,
                Type = Expense.ExpenseType.Hotel,
                Currency = "USD"
            };

            //check expense model is valid
            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(expense, new ValidationContext(expense), validationResults, true);
            Assert.True(actual);

            var response = await controller.PostExpense(expense);
            Assert.IsType<CreatedAtActionResult>(response.Result);

            dbContext.Dispose();
        }

        [Fact]
        public async Task TestWrongCurrencyPostExpenseAsync()
        {
            var dbContext = DbContextMocker.GetExpenseManagerDbContext(nameof(TestWrongCurrencyPostExpenseAsync));
            var controller = new ExpensesController(dbContext);

            //try to post the wrong currency
            var expense = new Expense
            {
                UserID = 1,
                Amount = 10,
                Comment = "This is a test comment",
                CreationDate = DateTime.Now,
                Type = Expense.ExpenseType.Hotel,
                Currency = "rouble américain"
            };

            var response = await controller.PostExpense(expense);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            var resultValue = badRequestResult.Value as SerializableError;
            Assert.True(resultValue.ContainsKey(ExpenseManager.Constants.ErrorTitle.IncorrectCurrency));
            dbContext.Dispose();
        }

        [Fact]
        public async Task TestExistingPostExpenseAsync()
        {
            var dbContext = DbContextMocker.GetExpenseManagerDbContext(nameof(TestExistingPostExpenseAsync));
            var controller = new ExpensesController(dbContext);

            var expense = new Expense
            {
                UserID = 1,
                Amount = 10,
                Comment = "This is a test comment",
                CreationDate = DateTime.Now,
                Type = Expense.ExpenseType.Hotel,
                Currency = "USD"
            };

            //try to post twice
            var response = await controller.PostExpense(expense);
            response = await controller.PostExpense(expense);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            var resultValue = badRequestResult.Value as SerializableError;
            Assert.True(resultValue.ContainsKey(ExpenseManager.Constants.ErrorTitle.IncorrectExpense));
            dbContext.Dispose();
        }

        [Fact]
        public async Task TestUnvalidUserPostExpenseAsync()
        {
            var dbContext = DbContextMocker.GetExpenseManagerDbContext(nameof(TestUnvalidUserPostExpenseAsync));
            var controller = new ExpensesController(dbContext);

            var expense = new Expense
            {
                UserID = 6,
                Amount = 10,
                Comment = "This is a test comment",
                CreationDate = DateTime.Now,
                Type = Expense.ExpenseType.Hotel,
                Currency = "USD"
            };

            //try to post twice
            var response = await controller.PostExpense(expense);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            var resultValue = badRequestResult.Value as SerializableError;
            Assert.True(resultValue.ContainsKey(ExpenseManager.Constants.ErrorTitle.IncorrectUser));
            dbContext.Dispose();
        }

        [Fact]
        public async Task TestUnvalidModelPostExpenseAsync()
        {
            var dbContext = DbContextMocker.GetExpenseManagerDbContext(nameof(TestUnvalidModelPostExpenseAsync));
            var controller = new ExpensesController(dbContext);

            controller.ModelState.AddModelError("FakeError", "This is a fake error");
            var expense = new Expense
            {
                UserID = 1,
                Amount = 10,
                Comment = "This is a test comment",
                CreationDate = DateTime.Now,
                Type = Expense.ExpenseType.Hotel,
                Currency = "USD"
            };            
            var response = await controller.PostExpense(expense);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            dbContext.Dispose();
        }

        [Fact]
        public void TestUnvalidDatePostExpenseAsync()
        {
            var expense = new Expense
            {
                UserID = 1,
                Amount = 10,
                Comment = "This is a test comment",
                CreationDate = DateTime.Now.AddDays(3),
                Type = Expense.ExpenseType.Hotel,
                Currency = "USD"
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(expense, new ValidationContext(expense), validationResults, true);

            Assert.False(actual, "Expected validation to fail.");
            Assert.Single(validationResults);
            var msg = validationResults[0];
            Assert.Equal(ExpenseManager.Constants.ErrorMessages.CreationDate, msg.ErrorMessage);
        }

        [Fact]
        public void TestUnvalidCommentPostExpenseAsync()
        {
            var expense = new Expense
            {
                UserID = 1,
                Amount = 10,
                Comment = "",
                CreationDate = DateTime.Now,
                Type = Expense.ExpenseType.Hotel,
                Currency = "USD"
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(expense, new ValidationContext(expense), validationResults, true);

            Assert.False(actual, "Expected validation to fail.");
            Assert.Single(validationResults);
            var msg = validationResults[0];
            Assert.Equal("The Comment field is required.", msg.ErrorMessage);
        }

        [Fact]
        public void TestMultipleErrorPostExpenseAsync()
        {
            var expense = new Expense
            {
                UserID = 1,
                Amount = 0,
                Comment = "",
                CreationDate = DateTime.Now.AddDays(3),
                Type = Expense.ExpenseType.Hotel,
                Currency = "USD"
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(expense, new ValidationContext(expense), validationResults, true);

            Assert.False(actual, "Expected validation to fail.");
            Assert.Equal(3,validationResults.Count);
        }


    }
}
