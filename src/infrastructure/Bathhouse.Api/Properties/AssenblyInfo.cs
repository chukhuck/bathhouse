using Chuk.Helpers.AspNetCore.ApiConvension;
using Microsoft.AspNetCore.Mvc;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
[assembly: ApiConventionType(typeof(DefaultDeleteApiConvension))]

