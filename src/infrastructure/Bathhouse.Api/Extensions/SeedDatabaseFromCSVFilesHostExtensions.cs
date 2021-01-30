using Bathhouse.EF.Data;
using Bathhouse.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Bathhouse.Api.Extensions
{
  public static class SeedDatabaseFromCSVFilesHostExtensions
  {
    public static void SeedDatabaseFromCSVFiles(this IHost host, ILogger logger)
    {
      try
      {
        string baseDirectory = Path.Combine(Path.GetDirectoryName(typeof(Program)?.Assembly?.Location) ?? string.Empty, "Seed");
        logger.LogInformation($"Base directory for seeding: {baseDirectory}.");

        using var scope = host.Services.CreateScope();
        using (BathhouseContext context = scope.ServiceProvider.GetRequiredService<BathhouseContext>())
        {
          if (!Directory.Exists(baseDirectory))
          {
            logger.LogError($"Folder SEED was not found in {Directory.GetCurrentDirectory()}.");
            return;
          }

          logger.LogInformation("Seeding is start.");

          if (File.Exists(Path.Combine(baseDirectory, "Office.csv")))
          {
            SeedOffices(baseDirectory, context, logger);
          }

          if (File.Exists(Path.Combine(baseDirectory, "Employee.csv")))
          {
            SeedEmployees(logger, baseDirectory, context);
          }
          context.SaveChanges();
        }

        logger.LogInformation("Seeding is finished.");
      }
      catch (Exception ex)
      {
        logger.LogError(ex, $"Error in {nameof(SeedDatabaseFromCSVFiles)}");
      }
    }

    private static void SeedEmployees(ILogger logger, string baseDirectory, BathhouseContext context)
    {
      try
      {
        logger.LogInformation($"File Employee.csv was found.");

        using StreamReader reader = new StreamReader(File.OpenRead(Path.Combine(baseDirectory, "Employee.csv")), Encoding.UTF8);

        CsvReader csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture)
        {
          Encoding = Encoding.UTF8,
          MissingFieldFound = (headerNames, index, context) => { return; },
          HeaderValidated = null
        });

        while (csvReader.Read())
        {
          Employee newEmployee = csvReader.GetRecord<Employee>();
          var officeNumbers = csvReader.GetField<string>("OfficeNumbers").Split(',', System.StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n));
          var roleNames = csvReader.GetField<string>("Roles").Split(',').Select(r => r.ToUpper().Trim()).ToList();
          logger.LogInformation($"Employees was read.");

          AddOfficesForEmployee(context, newEmployee, officeNumbers);

          context.Users.Add(newEmployee);

          AddRolesForEmployee(context, newEmployee, roleNames);
        }

        logger.LogInformation($"Employees was saved.");
      }
      catch (Exception)
      {
        throw;
      }   
    }

    private static void AddRolesForEmployee(BathhouseContext context, Employee newEmployee, List<string> roleNames)
    {
      try
      {
        foreach (var roleName in roleNames)
        {
          var role = context.Roles.FirstOrDefault(r => r.NormalizedName == roleName) ?? context.Roles.FirstOrDefault(r => r.NormalizedName == "EMPLOYEE");

          if (role != null)
            context.UserRoles.Add(new IdentityUserRole<Guid>() { RoleId = role.Id, UserId = newEmployee.Id });
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    private static void AddOfficesForEmployee(BathhouseContext context, Employee newEmployee, IEnumerable<int> officeNumbers)
    {
      try
      {
        newEmployee.Offices ??= new List<Office>();

        foreach (var officeNumber in officeNumbers)
        {
          Office? office = context.Offices.Local.Where(o => o.Number == officeNumber).FirstOrDefault();

          if (office is not null)
            newEmployee.Offices.Add(office);
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    private static void SeedOffices(string baseDirectory, BathhouseContext context, ILogger logger)
    {
      try
      {
        logger.LogInformation($"File Office.csv was found.");

        using StreamReader reader = new StreamReader(File.OpenRead(Path.Combine(baseDirectory, "Office.csv")), Encoding.UTF8);

        CsvReader csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture)
        {
          Encoding = Encoding.UTF8,
          MissingFieldFound = (headerNames, index, context) => { return; },
          HeaderValidated = null
        });

        var offices = csvReader.GetRecords<Office>().ToArray();
        logger.LogInformation($"Offices was read.");
        context.Offices.AddRange(offices);
        logger.LogInformation($"Offices was saved.");
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
