namespace PeopleManagement.DatabaseProvider.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public int PositionId { get; set; }
        public Position Position { get; set; }

        public int Salary { get; set; }

        public bool IsFired { get; set; }
    }
}
