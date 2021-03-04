using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Chuk.Helpers.AspNetCore
{
  public static class HttpContextExtension
  {
    public static string GetUserId(this HttpContext context)
    {
      if (context.User is null)
      {
        return string.Empty;
      }

      return context.User.Claims.Single(c => c.Type == "id").Value;
    }

    public static Guid GetGuidUserId(this HttpContext context)
    {
      string id = context.GetUserId();
      return new Guid(id);
    }

    public static int GetintUserId(this HttpContext context)
    {
      string id = context.GetUserId();
      return int.Parse(id);
    }
  }
}
