using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace WorthIt.Tests.Views.Analytics
{
    public class Expense
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

    public class Category
    {
        public string CategoryName { get; set; }
        public string CategoryIcon { get; set; }
    }

    public class User
    {
        public decimal HourlyWage { get; set; }
    }

    public class ExpenseItemViewModel
    {
        public Expense Expense { get; set; }
        public Category Category { get; set; }
        public User User { get; set; }
    }

    public class IndexViewLogicTests
    {
        private List<ExpenseItemViewModel> GetSampleData(DateTime today)
        {
            return new List<ExpenseItemViewModel>
            {
                new ExpenseItemViewModel
                {
                    Expense = new Expense { Date = today, Amount = 100 },
                    Category = new Category { CategoryName = "Food", CategoryIcon = "🍔" },
                    User = new User { HourlyWage = 20 },
                },
                new ExpenseItemViewModel
                {
                    Expense = new Expense { Date = today.AddDays(-1), Amount = 50 },
                    Category = new Category { CategoryName = "Food", CategoryIcon = "🍔" },
                    User = new User { HourlyWage = 20 },
                },
                new ExpenseItemViewModel
                {
                    Expense = new Expense { Date = today, Amount = 200 },
                    Category = new Category { CategoryName = "Transport", CategoryIcon = "🚗" },
                    User = new User { HourlyWage = 25 },
                },
                new ExpenseItemViewModel
                {
                    Expense = new Expense { Date = today.AddMonths(-1), Amount = 300 },
                    Category = new Category { CategoryName = "Old", CategoryIcon = "🕰️" },
                    User = new User { HourlyWage = 30 },
                },
            };
        }

        [Fact]
        public void FiltersExpensesForCurrentMonth()
        {
            var today = DateTime.Today;
            var model = GetSampleData(today);

            var monthExpenses = model
                .Where(e =>
                    e.Expense != null
                    && e.Expense.Date.Year == today.Year
                    && e.Expense.Date.Month == today.Month
                )
                .ToList();

            Assert.Equal(3, monthExpenses.Count);
            Assert.DoesNotContain(monthExpenses, e => e.Category.CategoryName == "Old");
        }

        [Fact]
        public void CalculatesTotalSpentThisMonth()
        {
            var today = DateTime.Today;
            var model = GetSampleData(today);

            var monthExpenses = model
                .Where(e =>
                    e.Expense != null
                    && e.Expense.Date.Year == today.Year
                    && e.Expense.Date.Month == today.Month
                )
                .ToList();

            var totalSpentThisMonth = monthExpenses.Sum(e => e.Expense.Amount);

            Assert.Equal(350, totalSpentThisMonth);
        }

        [Fact]
        public void GroupsByCategory()
        {
            var today = DateTime.Today;
            var model = GetSampleData(today);

            var monthExpenses = model
                .Where(e =>
                    e.Expense != null
                    && e.Expense.Date.Year == today.Year
                    && e.Expense.Date.Month == today.Month
                )
                .ToList();

            var groupedByCategory = monthExpenses
                .Where(e => e.Category != null)
                .GroupBy(e => new { e.Category.CategoryName, e.Category.CategoryIcon })
                .ToList();

            Assert.Equal(2, groupedByCategory.Count);
            Assert.Contains(groupedByCategory, g => g.Key.CategoryName == "Food");
            Assert.Contains(groupedByCategory, g => g.Key.CategoryName == "Transport");
        }

        [Fact]
        public void FindsHighestSpentDay()
        {
            var today = DateTime.Today;
            var model = GetSampleData(today);

            var monthExpenses = model
                .Where(e =>
                    e.Expense != null
                    && e.Expense.Date.Year == today.Year
                    && e.Expense.Date.Month == today.Month
                )
                .ToList();

            var highestDay = monthExpenses
                .GroupBy(e => e.Expense.Date.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(x => x.Expense.Amount) })
                .OrderByDescending(x => x.Total)
                .FirstOrDefault();

            Assert.NotNull(highestDay);
            Assert.Equal(today, highestDay.Date);
            Assert.Equal(300, highestDay.Total);
        }

        [Fact]
        public void FindsMostEntryCategory()
        {
            var today = DateTime.Today;
            var model = GetSampleData(today);

            var monthExpenses = model
                .Where(e =>
                    e.Expense != null
                    && e.Expense.Date.Year == today.Year
                    && e.Expense.Date.Month == today.Month
                )
                .ToList();

            var groupedByCategory = monthExpenses
                .Where(e => e.Category != null)
                .GroupBy(e => new { e.Category.CategoryName, e.Category.CategoryIcon })
                .ToList();

            var mostEntryCategory = groupedByCategory
                .Select(g => new
                {
                    g.Key.CategoryName,
                    g.Key.CategoryIcon,
                    Count = g.Count(),
                    Total = g.Sum(x => x.Expense.Amount),
                })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            Assert.NotNull(mostEntryCategory);
            Assert.Equal("Food", mostEntryCategory.CategoryName);
            Assert.Equal(2, mostEntryCategory.Count);
            Assert.Equal(150, mostEntryCategory.Total);
        }

        [Fact]
        public void CalculatesTimeTextForCategory()
        {
            var today = DateTime.Today;
            var model = GetSampleData(today);

            var monthExpenses = model
                .Where(e =>
                    e.Expense != null
                    && e.Expense.Date.Year == today.Year
                    && e.Expense.Date.Month == today.Month
                )
                .ToList();

            var groupedByCategory = monthExpenses
                .Where(e => e.Category != null)
                .GroupBy(e => new { e.Category.CategoryName, e.Category.CategoryIcon })
                .ToList();

            var foodGroup = groupedByCategory.First(g => g.Key.CategoryName == "Food");

            var anyWithUser = foodGroup.FirstOrDefault(x =>
                x.User != null && x.User.HourlyWage > 0 && x.Expense != null
            );

            string timeText = "";
            if (anyWithUser != null)
            {
                var hourly = anyWithUser.User.HourlyWage;
                var totalAmount = foodGroup
                    .Where(x => x.Expense != null)
                    .Sum(x => x.Expense.Amount);

                decimal hours = totalAmount / hourly;

                if (hours >= 1)
                {
                    var hoursString = hours.ToString("0.#", CultureInfo.InvariantCulture);
                    timeText = $"{hoursString}h";
                }
                else
                {
                    var minutesString = (hours * 60).ToString("0", CultureInfo.InvariantCulture);
                    timeText = $"{minutesString} min";
                }
            }

            Assert.Equal("7.5h", timeText);
        }
    }
}
