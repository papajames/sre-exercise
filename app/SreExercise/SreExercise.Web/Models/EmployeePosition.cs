using System.ComponentModel.DataAnnotations;

namespace SreExercise.Web.Models
{
    public class EmployeePosition
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        public string Department { get; set; }
    }
}