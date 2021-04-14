using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Data;
using ExpenseManager.Models;
namespace ExpenseManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpenseManagerContext _context;

        public ExpensesController(ExpenseManagerContext context)
        {
            _context = context;
        }


        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpense()
        {
            return await _context.Expenses.ToListAsync();
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            var expense = await _context.Expenses.Include(e => e.User).FirstOrDefaultAsync(m => m.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        // GET: api/Expenses/byUser?userid=1&sort=date
        [Route("byUser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenseByUser(int userid, string sort)
        {

            IQueryable<Expense> expensesIQ = _context.Expenses.Where(m => m.UserID == userid);
            if (!String.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "amount_desc":
                        expensesIQ = expensesIQ.OrderByDescending(s => s.Amount);
                        break;
                    case "date":
                        expensesIQ = expensesIQ.OrderBy(s => s.CreationDate);
                        break;
                    case "date_desc":
                        expensesIQ = expensesIQ.OrderByDescending(s => s.CreationDate);
                        break;
                    case "amount":
                        expensesIQ = expensesIQ.OrderBy(s => s.Amount);
                        break;
                    default:
                        break;
                }
            }
            var expenses = await expensesIQ.Select(e => new Expense
            {
                Id = e.Id,
                CreationDate = e.CreationDate,
                Amount = e.Amount,
                Currency = e.Currency,
                Type = e.Type,
                UserID = e.UserID,
                User = UserToDTO(_context.Users.Find(userid))
            }).AsNoTracking().ToListAsync();
            if (expenses == null)
            {
                return NotFound();
            }
            return expenses;
        }

        // PUT: api/Expenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }

            checkModelValidity(expense);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Expenses
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
            checkModelValidity(expense);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpense", new { id = expense.Id }, expense);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Expense>> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return expense;
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }

        private static UserDTO UserToDTO(User user) =>
        new UserDTO
        {
            Id = user.Id,
            FullName = user.FirstName + " " + user.LastName
        };

        private void checkModelValidity(Expense expense) {
            try
            {
                //check that the expense doesn't already exist
                var existing = _context.Expenses.Where(e => (e.UserID == expense.UserID && e.CreationDate == expense.CreationDate && e.Amount == expense.Amount)).FirstOrDefault();
                if (existing != null)
                {
                    ModelState.AddModelError(Constants.ErrorTitle.IncorrectExpense, Constants.ErrorMessages.IncorrectExpense);
                }
                //check same currency that user
                var userId = expense.UserID;
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    if (!String.Equals(user.Currency, expense.Currency, StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError(Constants.ErrorTitle.IncorrectCurrency, Constants.ErrorMessages.IncorrectCurrency);
                    }
                }
                else
                {
                    ModelState.AddModelError(Constants.ErrorTitle.IncorrectUser, Constants.ErrorMessages.IncorrectUser);
                }
            }
            catch (Exception) {
                ModelState.AddModelError(Constants.ErrorTitle.ModelError, Constants.ErrorMessages.ModelError);
            }
        }

    }
}
