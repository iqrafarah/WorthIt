using Microsoft.AspNetCore.Mvc;
using WorthIt.Data;
using WorthIt.Models;

namespace WorthIt.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AddExpense()
        {
            ViewData["HideHeader"] = true;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddExpense(decimal amount, string CategoryIcon, string CategoryName)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Amount must be greater than zero.");
                ViewData["HideHeader"] = true;
                return View();
            }

            var expense = new Expense
            {
                Amount = amount,
                Date = DateTime.Now,
                CategoryIcon = CategoryIcon,
                CategoryName = CategoryName,
            };

            _context.Expenses.Add(expense);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            var expenses = _context.Expenses.OrderByDescending(x => x.Date).ToList();

            return View(expenses);
        }
    }
}
