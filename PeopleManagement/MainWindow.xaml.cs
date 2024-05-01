using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PeopleManagement.DatabaseProvider;
using PeopleManagement.DatabaseProvider.Models;
using PeopleManagement.Services.Services;
using System;
using System.Linq;
using System.Windows;

namespace PeopleManagement
{
    public partial class MainWindow : Window
    {
        // To work with database
        private DataContext db;

        //For save correct employees list
        private string currentSearchText = "";

        // For reading json format filter
        private const string JsonFilter = "JSON files (*.json)|*.json|All files (*.*)|*.*";

        public MainWindow()
        {
            InitializeComponent();
            db = new DataContext();
            UpdateEmployeeListView();
        }

        // Load data from db
        private void LoadDataContext()
        {
            db.Employees.Load();
            db.Positions.Load();
        }

        // Invoke creation employee dialog
        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new AddEmployeeWindow();

            if (addEmployeeWindow.ShowDialog() == true)
            {
                UpdateEmployeeListView();
            }
        }

        // Fire employee
        private void FireEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeeListView.SelectedItem != null)
            {
                var selectedEmployee = (Employee)employeeListView.SelectedItem;
                selectedEmployee.IsFired = true;
                db.SaveChanges();

                if (!string.IsNullOrEmpty(currentSearchText))
                {
                    UpdateEmployeeListByFilter(currentSearchText);
                }
                else
                {
                    UpdateEmployeeListView();
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть співробітника для звільнення.");
            }
        }

        // To update list with fresh data
        private void UpdateEmployeeListView()
        {
            LoadDataContext();
            employeeListView.ItemsSource = db.Employees.Local.ToBindingList();
            employeeListView.Items.Refresh();
        }

        // To update list with fresh data by filter
        private void UpdateEmployeeListByFilter(string searchText)
        {
            var foundEmployees = db.Employees.
                                    Where(emp => emp.FullName.Contains(searchText) || emp.Id.ToString() == searchText).
                                    ToList();

            employeeListView.ItemsSource = foundEmployees;
            employeeListView.Items.Refresh();
        }

        // For search by parametr (FullName, Id)
        private void OnSearchTextChanged(object sender, RoutedEventArgs e)
        {
            var searchText = searchTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                currentSearchText = searchText;
                UpdateEmployeeListByFilter(searchText);
            }
            else
            {
                currentSearchText = string.Empty;
                UpdateEmployeeListView();
            }
        }

        // Get statistic
        private void GetStatisticButton_Click(object sender, RoutedEventArgs e)
        {
            var statisticsWindow = new StatisticsWindow();
            statisticsWindow.ShowDialog();
        }

        //Import json file
        private void ImportFromJsonButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = JsonFilter;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                SaveJsonDataFromFile(filePath);
                MessageBox.Show("Дані успішно імпортовано з JSON.");
            }
        }

        //Export employee list to json 
        private void ExporToJsonButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = JsonFilter;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                JsonDataService.ExportToJson(db.Employees.AsNoTracking().ToList(), filePath);
                MessageBox.Show("Дані успішно експортовано у JSON.");
            }
        }

        //Save json data to db
        private void SaveJsonDataFromFile(string filePath)
        {
            var employees = JsonDataService.ImportFromJson(filePath);

            foreach (var employee in employees)
            {
                db.Employees.Add(employee);
            }

            db.SaveChanges();
            UpdateEmployeeListView();
        }
    }
}
