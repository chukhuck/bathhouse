using System.Collections.Generic;

namespace Chuk.Helpers.AspNetCore
{
  public class PaginatedResponse<T>
  {
    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }

    public IEnumerable<T>? Data { get; set; }
  }
}
