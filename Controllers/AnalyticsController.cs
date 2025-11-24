using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WorthIt.Data;
using WorthIt.Models;

namespace WorthIt.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            var categories = _context.Categories.ToList();
            var expenses = _context.Expenses.ToList();

            var expenseItems = expenses
                .Select(e => new ExpenseItemViewModel
                {
                    Expense = e,
                    User = users.FirstOrDefault(u => u.Id == e.UserId),
                    Category = categories.FirstOrDefault(c => c.Id == e.CategoryId),
                })
                .ToList();

            return View(expenseItems);
        }
    }
}
