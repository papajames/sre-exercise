using System.Collections.Generic;
using System.Linq;
using SreExercise.Web.Models.Data;

namespace SreExercise.Web.Models
{
    public class AllEmployeesResponse
    {
        public IEnumerable<SingleEmployeeResponse> Employees { get; set; }

        public static AllEmployeesResponse FromData(IEnumerable<Employee> employees)
        {
            return new AllEmployeesResponse
            {
                Employees = employees.Select(SingleEmployeeResponse.FromData).ToArray()
            };
        }
    }
}