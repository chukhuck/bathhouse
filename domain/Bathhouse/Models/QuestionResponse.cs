using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Models
{
  public record QuestionResponse(
    Guid Id,
    string Text,
    string Name,
    bool IsKey);
}