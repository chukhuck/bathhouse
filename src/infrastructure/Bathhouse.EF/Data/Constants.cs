using System;
using System.Collections.Generic;

namespace Bathhouse.EF.Data
{
  public class Constants
  {
    public const string DirectorRoleName = "Director";
    public const string ManagerRoleName = "Manager";
    public const string EmployeeRoleName = "Employee";
    public const string AdminRoleName = "Admin";

    public static IEnumerable<string> GetBuildInRoleNormalizedNames()
    {
      yield return DirectorRoleName.ToUpper();
      yield return ManagerRoleName.ToUpper();
      yield return EmployeeRoleName.ToUpper();
      yield return AdminRoleName.ToUpper();
    }
  }
}
