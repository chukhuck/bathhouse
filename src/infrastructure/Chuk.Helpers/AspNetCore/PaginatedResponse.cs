using System.Collections.Generic;
using System.Linq;

namespace Chuk.Helpers.AspNetCore
{
  public class PaginatedResponse<T>
  {
    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }

    public int Count => Data?.Count() ?? default;

    public IEnumerable<T>? Data { get; set; }
  }
}
