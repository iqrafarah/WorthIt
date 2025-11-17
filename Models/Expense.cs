namespace WorthIt.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string CategoryIcon { get; set; } = "";

        public string CategoryName { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
    }
}
