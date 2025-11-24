using System;
using System.Linq;
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

            var categories = _context.Categories.ToList();

            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddExpense(decimal amount, int categoryId)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Amount must be greater than zero.");
                ViewData["HideHeader"] = true;

                var categories = _context.Categories.ToList();
                return View(categories);
            }

            var expense = new Expense
            {
                Amount = amount,
                Date = DateTime.Now,
                CategoryId = categoryId,
                UserId = 1,
            };

            _context.Expenses.Add(expense);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddCategory(string NewCategoryIcon, string NewCategoryName)
        {
            var category = new Category
            {
                CategoryName = NewCategoryName,
                CategoryIcon = NewCategoryIcon,
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            TempData["CategoryAddedMessage"] = "Category added successfully.";

            return RedirectToAction(nameof(AddExpense));
        }

        public IActionResult Index()
        {
            var expenses = _context.Expenses.ToList();
            var categories = _context.Categories.ToList();
            var users = _context.Users.ToList();

            var viewModelList = expenses
                .Select(expense => new ExpenseItemViewModel
                {
                    Expense = expense,
                    User = users.FirstOrDefault(u => u.Id == expense.UserId),
                    Category = categories.FirstOrDefault(c => c.Id == expense.CategoryId),
                })
                .ToList();

            return View(viewModelList);
        }
    }
}
