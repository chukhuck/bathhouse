using System.ComponentModel.DataAnnotations;

namespace Chuk.Helpers.AspNetCore
{
  public class PaginationQuery
  {
    public PaginationQuery() { }
    public PaginationQuery(int pageNumber, int pageSize)
    {
      PageSize = pageSize;
      PageNumber = pageNumber;
    }

    [Required]
    public int PageNumber { get; set; } = 1;

    [Required]
    public int PageSize { get; set; } = 100;
  }
}
