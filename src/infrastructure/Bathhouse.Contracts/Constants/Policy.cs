using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Contracts
{
  public partial class Constants
  {
    public const string OfficeModifyPolicy = "Office.Modify";

    public static IEnumerable<string> GetPoliciesName()
    {
      yield return OfficeModifyPolicy.ToUpper();
    }
  }
}
