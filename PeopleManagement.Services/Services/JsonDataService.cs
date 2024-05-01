using Newtonsoft.Json;
using PeopleManagement.DatabaseProvider.Models;
using System.Collections.Generic;
using System.IO;

namespace PeopleManagement.Services.Services
{
    public class JsonDataService
    {
        public static List<Employee> ImportFromJson(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            var employees = JsonConvert.DeserializeObject<List<Employee>>(jsonData);

            return employees;
        }

        public static void ExportToJson(List<Employee> employees, string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(employees, Formatting.Indented);

            File.WriteAllText(filePath, jsonData);
        }
    }
}
