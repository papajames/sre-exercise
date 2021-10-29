using SreExercise.Web.Models.Data;

namespace SreExercise.Web.Models
{
    public class SingleEmployeeResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public EmployeePosition Position { get; set; }

        public static SingleEmployeeResponse FromData(Employee employee)
        {
            return new SingleEmployeeResponse
            {
                Id = employee.Id,
                Email = employee.Email,
                Mobile = employee.Mobile,
                Position = new EmployeePosition
                {
                    Title = employee.Title,
                    Department = employee.Department
                }
            };
        }
    }
}