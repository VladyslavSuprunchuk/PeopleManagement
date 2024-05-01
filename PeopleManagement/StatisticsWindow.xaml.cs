using Microsoft.EntityFrameworkCore;
using PeopleManagement.DatabaseProvider;
using System.Linq;
using System.Windows;

namespace PeopleManagement
{
    public partial class StatisticsWindow : Window
    {
        // To work with database
        private DataContext db;

        public StatisticsWindow()
        {
            InitializeComponent();
            db = new DataContext();
            LoadDataContext();
            GetStatistics();
        }

        // Load data from db
        private void LoadDataContext()
        {
            db.Employees.Load();
            db.Positions.Load();
        }

        // Statistic setup
        private void GetStatistics()
        {
            var totalEmployees = db.Employees.Count();
            var averageSalary = db.Employees.Average(emp => emp.Salary);
            var employeesPerPosition = db.Employees
                                        .GroupBy(emp => emp.Position.Title)
                                        .Select(group => new
                                        {
                                            PositionName = group.Key,
                                            NumberOfEmployees = group.Count()
                                        }).ToList();

            positionStatisticsDataGrid.ItemsSource = employeesPerPosition;
            totalEmployeesTextBlock.Text = totalEmployees.ToString();
            averageSalaryTextBlock.Text = averageSalary.ToString("0.00");
        }

        // Window closing
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
