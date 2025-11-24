namespace WorthIt.Models
{
    public class User
    {
        public int Id { get; set; }

        public decimal MonthlyIncome { get; set; } = 0;

        public decimal HourlyWage { get; set; } = 0;

        public string Role { get; set; } = "User";
    }
}
