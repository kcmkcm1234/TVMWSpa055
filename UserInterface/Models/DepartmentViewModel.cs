using System.ComponentModel.DataAnnotations;

namespace UserInterface.Models
{
    public class DepartmentViewModel
    {
        [Display(Name = "Code")]
        [Required(ErrorMessage = "Please Enter Code")]
        [MaxLength(10)]
        public string Code { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please Enter Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        public CommonViewModel commonObj { get; set; }
        public string Operation { get; set; }
    }
}