using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Chuk.Helpers.AspNetCore.ApiConvension
{
  public static class DefaultDeleteApiConvension
  {
#pragma warning disable IDE0060
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
    public static void Delete([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Suffix)] int id) { }
  }
}
