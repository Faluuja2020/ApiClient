using System;
using Serilog;
using System.Threading.Tasks;
using EmployeRazor.Models; // Add this namespace

namespace EmployeeApiClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Starting application");

            try
            {
                // Create an instance of the client
                var employeeClient = new EmployeeClient();
                await employeeClient.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while running the EmployeeClient.");
            }
            finally
            {
                Log.Information("Application finished");
                Log.CloseAndFlush();
            }
        }
    }
}
