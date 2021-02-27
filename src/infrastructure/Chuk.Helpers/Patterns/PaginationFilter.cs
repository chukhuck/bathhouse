using System;

namespace Chuk.Helpers.Patterns
{
  public class PaginationFilter
  {
    public PaginationFilter() { }
    
    public int PageNumber { get; set; } = 0;

    public int PageSize { get; set; } = 100;

    public bool IsValid => PageNumber > 0 && PageSize > 0;
    
  }
}
