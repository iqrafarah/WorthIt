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
        public IActionResult AddExpense(
            decimal amount,
            int categoryId,
            string CategoryIcon,
            string CategoryName
        )
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
                CategoryId = categoryId,
            };

            var category = new Category
            {
                CategoryName = CategoryName,
                CategoryIcon = CategoryIcon,
            };

            _context.Expenses.Add(expense);
            _context.Categories.Add(category);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            var expenses = _context.Expenses.ToList();
            var categories = _context.Categories.ToList();

            var viewModelList = expenses
                .Select(expense => new ExpenseItemViewModel
                {
                    Expense = expense,
                    Category = categories.FirstOrDefault(c => c.Id == expense.CategoryId),
                })
                .ToList();

            return View(viewModelList);
        }
    }
}
