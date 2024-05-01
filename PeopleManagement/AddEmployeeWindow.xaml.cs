using Microsoft.EntityFrameworkCore;
using PeopleManagement.DatabaseProvider;
using PeopleManagement.DatabaseProvider.Models;
using System.Windows;

namespace PeopleManagement
{
    public partial class AddEmployeeWindow : Window
    {
        // To work with database
        private DataContext db;

        public AddEmployeeWindow()
        {
            InitializeComponent();
            db = new DataContext();
            LoadDataContext();
            Setup();
        }

        // Load data from db
        private void LoadDataContext()
        {
            db.Positions.Load();
        }

        // Setup to create employee 
        private void Setup()
        {
            positionComboBox.ItemsSource = db.Positions.Local.ToBindingList();
            positionComboBox.SelectedIndex = 0;
        }

        // To create new position
        private void NewPositionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            newPositionTextBox.Visibility = Visibility.Visible;
            positionComboBox.IsEnabled = false;
        }

        // To select existing position
        private void NewPositionCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            newPositionTextBox.Visibility = Visibility.Collapsed;
            positionComboBox.IsEnabled = true;
        }

        // Create employee event
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var isEmployeeValid = CheckIsEmployeeValid();

            if (isEmployeeValid)
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.",
                                "Помилка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            CreateEmployee();

            DialogResult = true;
            Close();
        }

        // Validate input parametrs
        private bool CheckIsEmployeeValid()
        {
            var isEmployeeValid = string.IsNullOrWhiteSpace(fullNameTextBox.Text) ||
               (newPositionCheckBox.IsChecked == true && string.IsNullOrEmpty(newPositionTextBox.Text)) ||
               string.IsNullOrWhiteSpace(salaryTextBox.Text);

            return isEmployeeValid;
        }

        // Window closing
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // New employee creation
        private void CreateEmployee()
        {
            var newEmployee = new Employee
            {
                FullName = fullNameTextBox.Text,
                Salary = int.Parse(salaryTextBox.Text),
                IsFired = false
            };

            if (newPositionCheckBox.IsChecked == true)
            {
                var position = new Position()
                {
                    Title = newPositionTextBox.Text
                };

                db.Positions.Add(position);
                newEmployee.Position = position;
            }
            else
            {
                newEmployee.Position = positionComboBox.SelectedItem as Position;
            }

            db.Employees.Add(newEmployee);
            db.SaveChanges();
        }
    }
}
