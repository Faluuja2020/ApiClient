using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using EmployeRazor.Models;
using System.Net.Http.Json;
using EmployeRazor.Models;

namespace EmployeeApiClient
{
    public class EmployeeClient
    {
        private static readonly HttpClient client = new HttpClient();

        public EmployeeClient()
        {
            // Set base address and default headers
            client.BaseAddress = new Uri("https://localhost:7257/api/Employees");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task RunAsync()
        {
            try
            {
                // Get all employees
                var employees = await GetEmployeesAsync();
                foreach (var employee in employees)
                {
                    Log.Information($"ID: {employee.Id}, Name: {employee.Name}, Email: {employee.Email}, Age: {employee.Age}, Position: {employee.Position}");
                }

                // Get an employee by ID
                var employeeById = await GetEmployeeByIdAsync(1);
                Log.Information($"Fetched employee: {employeeById.Name}");

                // Create a new employee
                var newEmployee = new Employee { Name = "John Doe", Email = "john.doe@example.com", Age = 30, Position = "Developer" };
                var createdEmployee = await CreateEmployeeAsync(newEmployee);
                Log.Information($"Created employee: {createdEmployee.Name}");

                // Update an employee
                createdEmployee.Position = "Senior Developer";
                await UpdateEmployeeAsync(createdEmployee.Id, createdEmployee);
                Log.Information($"Updated employee: {createdEmployee.Name}");

                // Delete an employee
                await DeleteEmployeeAsync(createdEmployee.Id);
                Log.Information("Deleted employee");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the employees.");
            }
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            var response = await client.GetAsync("Employees");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Employee>>(responseBody);
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var response = await client.GetAsync($"Employees/{id}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Employee>(responseBody);
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            var response = await client.PostAsJsonAsync("Employees", employee);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Employee>(responseBody);
        }

        public async Task UpdateEmployeeAsync(int id, Employee employee)
        {
            var response = await client.PutAsJsonAsync($"Employees/{id}", employee);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var response = await client.DeleteAsync($"Employees/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
