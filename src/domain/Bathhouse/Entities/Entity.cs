using System;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse
{
  public class Entity
  {
    [Required]
    [Key()]
    public Guid Id { get; set; } = Guid.NewGuid();
  }
}