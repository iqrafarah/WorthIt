namespace WorthIt.Models
{
    public class Expense
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = "";
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }

        public decimal HoursCost { get; set; } = 0;
        public decimal IncomePercentage { get; set; } = 0;
    }
}
