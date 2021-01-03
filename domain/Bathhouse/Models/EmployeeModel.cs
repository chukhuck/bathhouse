using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Models
{
  public class EmployeeModel : EntityModel
  {
    [Required]
    [DefaultValue("Фамилия")]
    public string LastName { get; set; } = "Фамилия";
  }
}
