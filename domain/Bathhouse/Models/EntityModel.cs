using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Models
{
  public class EntityModel
  {
    /// <summary>
    /// ID
    /// </summary>
    [Required]
    public Guid Id { get; set; }
  }
}
