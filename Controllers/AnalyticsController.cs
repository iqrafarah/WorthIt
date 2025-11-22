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
            var expenseItems = _context
                .Expenses.Select(e => new ExpenseItemViewModel
                {
                    Expense = e,
                    Category = _context.Categories.FirstOrDefault(c => c.Id == e.CategoryId),
                })
                .ToList();
            return View(expenseItems);
        }
    }
}
