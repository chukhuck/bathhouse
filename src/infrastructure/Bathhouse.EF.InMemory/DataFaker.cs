using Bathhouse.Contracts;
using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.ValueTypes;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.EF.InMemory
{
  public class DataFaker
  {
    public static void Generate(BathhouseContext context, DataFakerOption opt)
    {
      Randomizer.Seed = new Random(8675309);

      var userStore = new UserStore<Employee, IdentityRole<Guid>, BathhouseContext, Guid>(context);


      var director = GenerateDirector(context, opt);
      userStore.AddToRoleAsync(director, Constants.DirectorRoleName.ToUpper()).Wait();


      var admin = GenerateTechSupporter(context, opt);
      userStore.AddToRoleAsync(admin, Constants.AdminRoleName.ToUpper()).Wait();

      var managers = GenerateManagers(context, opt);
      foreach (var manager in managers)
      {
        userStore.AddToRoleAsync(manager, Constants.ManagerRoleName.ToUpper()).Wait();
      }

      var employees = GenerateOtherEmployees(context, opt);
      foreach (var employee in employees)
      {
        userStore.AddToRoleAsync(employee, Constants.EmployeeRoleName.ToUpper()).Wait();
      }

      GenerateWorkItems(context, opt, creator: director);

      foreach (var survey in GenerateSurveys(context, opt: opt, author: director))
      {
        foreach (var result in GenerateSurveyResults(context, opt, survey: survey))
        {
          foreach (var question in survey.Questions)
          {
            GenerateAnswers(context, opt, count: 1, result: result, question: question);
          }
        }
      }

      context.SaveChanges();
    }



    private static List<Survey> GenerateSurveys(BathhouseContext context, Employee author, DataFakerOption opt)
    {
      var testSurveys = new Faker<Survey>(opt.Locale)
        .RuleFor(a => a.Author, (f, o) => author)
        .RuleFor(a => a.AuthorId, f => author.Id)
        .RuleFor(a => a.CreationDate, f => f.Date.Between(DateTime.Parse(opt.Common_start_day), DateTime.Parse(opt.Common_end_day)))
        .RuleFor(a => a.Description, f => "Description " + f.IndexFaker.ToString())
        .RuleFor(a => a.Name, f => "Survey " + f.IndexFaker.ToString())
        .RuleFor(a => a.Status, f => f.PickRandom<SurveyStatus>())
        .RuleFor(a => a.Questions, (f, o) => GenerateQuestions(
          context,
          count: f.Random.Number(opt.Min_Count_Of_Question_In_Survey, opt.Max_Count_Of_Question_In_Survey),
          survey: o,
          opt))
        .RuleFor(a => a.Results, (f, o) => new List<SurveyResult>())
        ;

      var newSurveys = testSurveys.Generate(opt.Count_Of_Surveys).ToList();
      context.Surveys.AddRange(newSurveys);
      return newSurveys;
    }

    private static List<Question> GenerateQuestions(BathhouseContext context, int count, Survey survey, DataFakerOption opt)
    {
      var testQuestions = new Faker<Question>(opt.Locale)
        .RuleFor(a => a.Name, f => "Column " + f.IndexFaker.ToString())
        .RuleFor(a => a.Type, f => f.PickRandom<QuestionType>())
        .RuleFor(a => a.Text, (f, o) => "Question " + f.IndexFaker)
        .RuleFor(a => a.IsKey, f => f.Random.Bool())
        .RuleFor(a => a.Survey, f => survey)
        .RuleFor(a => a.SurveyId, (f, o) => survey.Id)
        .RuleFor(a => a.Answers, (f, o) => new List<Answer>())
        ;

      var newQuestions = testQuestions.Generate(count).ToList();
      context.Questions.AddRange(newQuestions);
      return newQuestions;
    }

    private static string GenerateOneAnswer(QuestionType type, Faker faker, DataFakerOption opt)
    {
      return type switch
      {
        QuestionType.Text => "Answer " + faker.IndexFaker,
        QuestionType.YesNo => faker.Random.Bool().ToString(),
        QuestionType.Number => faker.Random.Number(opt.Min_Number_In_Answer, opt.Max_Number_In_Answer).ToString(),
        QuestionType.Decimal => faker.Random.Decimal(opt.Min_Number_In_Answer, opt.Max_Number_In_Answer).ToString(),
        QuestionType.Photo => Convert.ToBase64String(faker.Random.Bytes(10)),
        QuestionType.DateTime => faker.Date.Between(DateTime.Parse(opt.Common_start_day), DateTime.Parse(opt.Common_end_day)).ToString(),
        _ => throw new ArgumentException("Unrecognized question type")
      };
    }

    private static List<SurveyResult> GenerateSurveyResults(BathhouseContext context, DataFakerOption opt, Survey survey)
    {
      var testSurveyResults = new Faker<SurveyResult>(opt.Locale)
        .RuleFor(a => a.CreationDate, f => f.Date.Between(
          DateTime.Parse(opt.Common_start_day),
          DateTime.Parse(opt.Common_end_day)))
        .RuleFor(a => a.Author, (f, o) => f.PickRandom(context.Users.Local.ToList()))
        .RuleFor(a => a.AuthorId, (f, o) => o.Author?.Id ?? null)
        .RuleFor(a => a.Survey, (f, o) => survey)
        .RuleFor(a => a.SurveyId, (f, o) => survey.Id)
        .RuleFor(a => a.Answers, (f, o) => new List<Answer>())
        ;

      var newSurveyResults = testSurveyResults.Generate(opt.Count_Of_SurveyResult).ToList();
      context.SurveyResults.AddRange(newSurveyResults);

      return newSurveyResults;
    }

    private static List<Answer> GenerateAnswers(BathhouseContext context, DataFakerOption opt, int count, SurveyResult result, Question question)
    {
      var testAnswers = new Faker<Answer>(opt.Locale)
        .RuleFor(a => a.Question, (f, o) => question)
        .RuleFor(a => a.QuestionId, (f, o) => o.Question.Id)
        .RuleFor(a => a.Value, (f, o) => GenerateOneAnswer(o.Question.Type, f, opt))
        .RuleFor(a => a.Result, (f, o) => result)
        .RuleFor(a => a.ResultId, (f, o) => result.Id)
        ;

      var newAnswers = testAnswers.Generate(count).ToList();
      context.Answers.AddRange(newAnswers);
      return newAnswers;
    }

    private static Employee GenerateTechSupporter(BathhouseContext context, DataFakerOption opt)
    {
      Employee techsupporter = new()
      {
        DoB = DateTime.Parse(opt.TechSupport_DoB),
        LastName = opt.TechSupport_LastName,
        FirstName = opt.TechSupport_FirstName,
        MiddleName = opt.TechSupport_MiddleName,
        PhoneNumber = opt.TechSupport_PhoneNumber,
        Email = opt.TechSupport_Email,
        Offices = new List<Office>(),
        SurveyResults = new List<SurveyResult>(),
        WorkItems = new List<WorkItem>(),
        CreatedWorkItems = new List<WorkItem>(),
        SecurityStamp = Guid.NewGuid().ToString(),
        NormalizedEmail = opt.TechSupport_Email.ToUpper(),
        UserName = opt.TechSupport_Email,
        NormalizedUserName = opt.TechSupport_Email.ToUpper()
      };

      context.Users.Add(techsupporter);

      return techsupporter;
    }

    private static Employee GenerateDirector(BathhouseContext context, DataFakerOption opt)
    {
      Employee director = new()
      {
        DoB = DateTime.Parse(opt.Director_DoB),
        LastName = opt.Director_LastName,
        FirstName = opt.Director_FirstName,
        MiddleName = opt.Director_MiddleName,
        PhoneNumber = opt.Director_PhoneNumber,
        Email = opt.Director_Email,
        Offices = new List<Office>(),
        SurveyResults = new List<SurveyResult>(),
        WorkItems = new List<WorkItem>(),
        CreatedWorkItems = new List<WorkItem>(),
        SecurityStamp = Guid.NewGuid().ToString(),
        NormalizedEmail = opt.Director_Email.ToUpper(),
        UserName = opt.Director_Email,
        NormalizedUserName = opt.Director_Email.ToUpper()
      };

      GenerateWorkItems(context, opt, creator: director, executor: director);

      context.Users.Add(director);

      return director;
    }

    private static List<WorkItem> GenerateWorkItems(BathhouseContext context, DataFakerOption opt, Employee creator, Employee? executor = null)
    {
      var testWorkItems = new Faker<WorkItem>(opt.Locale)
        .RuleFor(a => a.CreationDate, f => f.Date.Between(
          DateTime.Parse(opt.Common_start_day),
          DateTime.Parse(opt.Common_end_day)))
        .RuleFor(a => a.StartDate, f => f.Date.Between(
          DateTime.Parse(opt.Common_start_day),
          DateTime.Parse(opt.Common_end_day)))
        .RuleFor(a => a.EndDate, f => f.Date.Between(
          DateTime.Parse(opt.Common_start_day),
          DateTime.Parse(opt.Common_end_day)))
        .RuleFor(a => a.Description, f => "Description " + f.IndexFaker.ToString())
        .RuleFor(a => a.Status, f => f.PickRandom<WorkItemStatus>())
        .RuleFor(a => a.IsImportant, f => f.Random.Bool())
        .RuleFor(a => a.Creator, f => creator)
        .RuleFor(a => a.CreatorId, (f, o) => creator.Id)
        .RuleFor(a => a.Executor, (f, o) => executor ?? f.PickRandom(context.Users.Local.ToList()))
        .RuleFor(a => a.ExecutorId, (f, o) => o.Executor?.Id)
        ;

      var newWorkItems = testWorkItems.Generate(opt.Count_Of_WorkItem).ToList();
      context.WorkItems.AddRange(newWorkItems);

      return newWorkItems;
    }

    private static List<Client> GenerateClients(BathhouseContext context, DataFakerOption opt, Office office)
    {
      var testClients = new Faker<Client>(opt.Locale)
        .RuleFor(a => a.Comment, f => f.Lorem.Sentence())
        .RuleFor(a => a.DoB, f => f.Date.Between(
          DateTime.Parse(opt.Start_birthday),
          DateTime.Parse(opt.End_birthday)))
        .RuleFor(a => a.LastName, f => f.Person.LastName)
        .RuleFor(a => a.MiddleName, f => "Отчество")
        .RuleFor(a => a.FirstName, f => f.Person.FirstName)
        .RuleFor(a => a.Phone, f => f.Person.Phone)
        .RuleFor(a => a.Sex, f => f.PickRandom<Sex>())
        .RuleFor(a => a.Office, f => office)
        .RuleFor(a => a.OfficeId, (f, o) => office.Id)
        ;

      var newClients = testClients.Generate(opt.Count_clients_per_office).ToList();

      context.Clients.AddRange(newClients);

      return newClients;
    }

    private static List<Employee> GenerateManagers(BathhouseContext context, DataFakerOption opt)
    {
      var testEmployees = new Faker<Employee>(opt.Locale)
        .RuleFor(a => a.DoB, f => f.Date.Between(
          DateTime.Parse(opt.Start_birthday),
          DateTime.Parse(opt.End_birthday)))
        .RuleFor(a => a.LastName, f => f.Person.LastName)
        .RuleFor(a => a.FirstName, f => f.Person.FirstName)
        .RuleFor(a => a.MiddleName, f => "Отчество")
        .RuleFor(a => a.PhoneNumber, f => f.Person.Phone)
        .RuleFor(a => a.Email, f => f.Person.Email)
        .RuleFor(a => a.Offices, (f, o) => GenerateOffices(
          context,
          opt,
          f.Random.Number(opt.Min_count_of_office_for_manager, opt.Max_count_of_office_for_manager),
          new List<Employee>() { o }))
        .RuleFor(a => a.WorkItems, (f, o) => context.WorkItems.Local.Where(wi => wi.ExecutorId == o.Id).ToList())
        .RuleFor(a => a.CreatedWorkItems, (f, o) => context.WorkItems.Local.Where(wi => wi.CreatorId == o.Id).ToList())
        .RuleFor(a => a.SurveyResults, f => new List<SurveyResult>())
        .RuleFor(m => m.SecurityStamp, f => Guid.NewGuid().ToString())
        .RuleFor(m=>m.NormalizedEmail, (f, o)=> o.Email.ToUpper())
        .RuleFor(m => m.UserName, (f, o) => o.Email)
        .RuleFor(m => m.NormalizedUserName, (f, o) => o.Email.ToUpper())
        ;

      var newEmployeees = testEmployees.Generate(opt.Count_of_managers).ToList();

      context.Users.AddRange(newEmployeees);

      return newEmployeees;
    }

    private static List<Employee> GenerateOtherEmployees(BathhouseContext context, DataFakerOption opt)
    {
      var testEmployees = new Faker<Employee>(opt.Locale)
        .RuleFor(a => a.DoB, f => f.Date.Between(
          DateTime.Parse(opt.Start_birthday),
          DateTime.Parse(opt.End_birthday)))
        .RuleFor(a => a.LastName, f => f.Person.LastName)
        .RuleFor(a => a.FirstName, f => f.Person.FirstName)
        .RuleFor(a => a.MiddleName, f => "Отчество")
        .RuleFor(a => a.PhoneNumber, f => f.Person.Phone)
        .RuleFor(a => a.Email, f => f.Person.Email)
        .RuleFor(a => a.Offices, (f, o) =>
        {
          var randomOffice = f.PickRandom(context.Offices.Local.ToList());
          return new List<Office>() { randomOffice };
        })
        .RuleFor(a => a.WorkItems, (f, o) => context.WorkItems.Local.Where(wi => wi.ExecutorId == o.Id).ToList())
        .RuleFor(a => a.CreatedWorkItems, (f, o) => context.WorkItems.Local.Where(wi => wi.CreatorId == o.Id).ToList())
        .RuleFor(a => a.SurveyResults, f => new List<SurveyResult>())
        .RuleFor(m => m.SecurityStamp, f => Guid.NewGuid().ToString())
        .RuleFor(m => m.NormalizedEmail, (f, o) => o.Email.ToUpper())
        .RuleFor(m => m.UserName, (f, o) => o.Email)
        .RuleFor(m => m.NormalizedUserName, (f, o) => o.Email.ToUpper())
        ;

      var newEmployeees = testEmployees.Generate(opt.Count_Of_Eemployees).ToList();

      context.Users.AddRange(newEmployeees);

      return newEmployeees;
    }

    private static List<Office> GenerateOffices(BathhouseContext context, DataFakerOption opt, int count, List<Employee> employees)
    {
      var testOffices = new Faker<Office>(opt.Locale)
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.TimeOfOpen, f => f.Date.Between(
          DateTime.MinValue.AddHours(opt.Min_Hour_Of_Openning_Office),
          DateTime.MinValue.AddHours(opt.Max_Hour_Of_Openning_Office)))
        .RuleFor(a => a.TimeOfClose, f => f.Date.Between(
          DateTime.MinValue.AddHours(opt.Min_Hour_Of_Closing_Office),
          DateTime.MinValue.AddHours(opt.Max_Hour_Of_Closing_Office)))
        .RuleFor(a => a.Phone, f => f.Person.Phone)
        .RuleFor(a => a.Email, f => f.Person.Email)
        .RuleFor(a => a.Address, f => f.Address.FullAddress())
        .RuleFor(a => a.Number, f => f.Random.Number(opt.Min_number_of_office, opt.Max_number_of_office))
        .RuleFor(a => a.Clients, (f, o) => GenerateClients(context, opt, o))
        .RuleFor(a => a.Employees, f => employees)
        ;

      var newOffices = testOffices.Generate(count).ToList();

      context.Offices.AddRange(newOffices);

      return newOffices;
    }

  }
}
