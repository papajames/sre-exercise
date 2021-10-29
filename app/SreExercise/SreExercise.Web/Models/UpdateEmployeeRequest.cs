using System.ComponentModel.DataAnnotations;
using SreExercise.Web.Models.Data;

namespace SreExercise.Web.Models
{
    public class UpdateEmployeeRequest
    {
        [Required]
        [RegularExpression(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$")]
        [StringLength(50)]
        public string Email { get; set; }
        
        [Required]
        [RegularExpression(@"^09[0-9]{8}$")]
        public string Mobile { get; set; }

        [Required]
        public EmployeePosition Position { get; set; }

        public void UpdateEmployee(Employee employee)
        {
            employee.Email = Email;
            employee.Mobile = Mobile;
            employee.Title = Position.Title;
            employee.Department = Position.Department;
        }
    }
}