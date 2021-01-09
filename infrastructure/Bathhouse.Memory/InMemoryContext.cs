using Bathhouse.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Memory
{
  public static class InMemoryContext
  {
    private const int Count_of_managers = 15;
    private const int Count_Of_Eemployees = 300;
    private const string Locale = "ru";
    private const int Count_Of_WorkItem = 50;
    private const int Count_Of_Surveys = 30;
    private const int Count_Of_ServeyResult = 20;
    private const int Min_Count_Of_Question_In_Survey = 3;
    private const int Max_Count_Of_Question_In_Survey = 5;
    private const int Min_Answer_Value = 0;
    private const int Max_Answer_Value = 100;
    private const int Count_MyWorkItems = 10;
    private const int Min_count_of_office_for_manager = 1;
    private const int Max_count_of_office_for_manager = 3;
    private const int Count_clients_per_office = 100;
    private const int Min_number_of_office = 1;
    private const int Max_number_of_office = 2000;
    private const string common_start_day = "2020-01-01";
    private const string common_end_day = "2021-01-07";
    private const string start_birthday = "1945-01-01";
    private const string end_birthday = "2002-01-01";
    private const int Min_Hour_Of_Openning_Office = 0;
    private const int Max_Hour_Of_Openning_Office = 12;
    private const int Min_Hour_Of_Closing_Office = 13;
    private const int Max_Hour_Of_Closing_Office = 24;

    public static readonly List<Answer> Answers = new List<Answer>();
    public static readonly List<Client> Clients = new List<Client>();
    public static readonly List<Employee> Employees = new List<Employee>();
    public static readonly List<Office> Offices = new List<Office>();
    public static readonly List<Question> Questions = new List<Question>();
    public static readonly List<Survey> Surveys = new List<Survey>();
    public static readonly List<SurveyResult> SurveyResults = new List<SurveyResult>();
    public static readonly List<WorkItem> WorkItems = new List<WorkItem>();

    internal static List<TEntity> Init<TEntity>(List<TEntity> entities) where TEntity : Entity
    {
      switch (entities)
      {
        case List<Answer>: return Answers.Cast<TEntity>().ToList();
        case List<Client>: return Clients.Cast<TEntity>().ToList();
        case List<Employee>: return Employees.Cast<TEntity>().ToList();
        case List<Office>: return Offices.Cast<TEntity>().ToList();
        case List<Question>: return Questions.Cast<TEntity>().ToList();
        case List<Survey>: return Surveys.Cast<TEntity>().ToList();
        case List<SurveyResult>: return SurveyResults.Cast<TEntity>().ToList();
        case List<WorkItem>: return WorkItems.Cast<TEntity>().ToList();
        default: throw new ArgumentException("Unknown type of entity.");
      }
    }

    static InMemoryContext()
    {
      Randomizer.Seed = new Random(8675309);

      var director = GenerateDirector();
      GenerateTechSupporter();
      GenerateManagers(locale: Locale, count: Count_of_managers);
      GenerateOtherEmployees(locale: Locale, count: Count_Of_Eemployees);

      director.CreatedWorkItems = GenerateWorkItems(locale: InMemoryContext.Locale, count: Count_Of_WorkItem, creator: director);

      foreach (var survey in GenerateSurveys(locale: Locale, count: Count_Of_Surveys))
      {
        foreach (var result in GenerateSurveyResults(locale: Locale, count: Count_Of_ServeyResult, survey: survey))
        {
          foreach (var question in survey.Questions)
          {
            GenerateAnswers(locale: Locale, 1, result: result, question: question);
          }
        }
      }
    }

    private static List<Survey> GenerateSurveys(string locale, int count)
    {
      var testSurveys = new Faker<Survey>(locale)
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.CreationDate, f => f.Date.Between(DateTime.Parse(common_start_day), DateTime.Parse(common_end_day)))
        .RuleFor(a => a.Description, f => "Description " + f.IndexFaker.ToString())
        .RuleFor(a => a.Name, f => "Survey " + f.IndexFaker.ToString())
        .RuleFor(a => a.Questions, (f, o) => GenerateQuestions(locale: locale, count: f.Random.Number(Min_Count_Of_Question_In_Survey, Max_Count_Of_Question_In_Survey), survey: o))
        .RuleFor(a => a.Results, (f, o) => new List<SurveyResult>())
        ;

      var newSurveys = testSurveys.Generate(count).ToList();
      Surveys.AddRange(newSurveys);
      return newSurveys;
    }

    private static List<Question> GenerateQuestions(string locale, int count, Survey survey)
    {
      var testQuestions = new Faker<Question>(locale)
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.Name, f => "Column " + f.IndexFaker.ToString())
        .RuleFor(a => a.Text, f => "Question " + f.IndexFaker.ToString())
        .RuleFor(a => a.IsKey, f => f.Random.Bool())
        .RuleFor(a => a.Survey, f => survey)
        .RuleFor(a => a.SurveyId, (f, o) => survey.Id)
        .RuleFor(a => a.Answers, (f, o) => new List<Answer>())
        ;

      var newQuestions = testQuestions.Generate(count).ToList();
      Questions.AddRange(newQuestions);
      return newQuestions;
    }

    private static List<SurveyResult> GenerateSurveyResults(string locale, int count, Survey survey)
    {
      var testSurveyResults = new Faker<SurveyResult>(locale)
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.CreationDate, f => f.Date.Between(DateTime.Parse(common_start_day), DateTime.Parse(common_end_day)))
        .RuleFor(a => a.Author, (f,o) => {

          var randomEmployee = f.PickRandom(Employees);
          randomEmployee.SurveyResults.Add(o);
          return randomEmployee;
          })
        .RuleFor(a => a.AuthorId, (f, o) => o.Author.Id)
        .RuleFor(a => a.Survey, (f,o) =>
        {
          survey.Results.Add(o);
          return survey;
          })
        .RuleFor(a => a.SurveyId, (f, o) => survey.Id)
        .RuleFor(a => a.Answers, (f, o) => new List<Answer>())
        ;

      var newSurveyResults = testSurveyResults.Generate(count).ToList();
      SurveyResults.AddRange(newSurveyResults);

      return newSurveyResults;
    }

    private static List<Answer> GenerateAnswers(string locale, int count, SurveyResult result, Question question)
    {
      var testAnswers = new Faker<Answer>(locale)
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.Value, f => f.Random.Double(Min_Answer_Value, Max_Answer_Value).ToString())
        .RuleFor(a => a.Question, (f,o) => {
          question.Answers.Add(o);
          return question;
          })
        .RuleFor(a => a.QuestionId, (f, o) => o.Question.Id)
        .RuleFor(a => a.Result, (f, o) => {
          result.Answers.Add(o);
          return result;
        })
        .RuleFor(a => a.ResultId, (f, o) => result.Id)
        ;

      var newAnswers = testAnswers.Generate(count).ToList();
      Answers.AddRange(newAnswers);
      return newAnswers;
    }

    private static Employee GenerateTechSupporter()
    {
      Employee techsupporter = new()
      {
        Id = Guid.NewGuid(),
        DoB = DateTime.Parse("1989-11-12"),
        LastName = "Потапчук",
        FirstName = "Александр",
        Phone = "916-099-68-36",
        Type = EmployeeType.TechnicalSupport,
        Offices = new List<Office>(),
        SurveyResults = new List<SurveyResult>(),
        WorkItems = new List<WorkItem>(),
        CreatedWorkItems = new List<WorkItem>()
      };

      Employees.Add(techsupporter);

      return techsupporter;
    }

    private static Employee GenerateDirector()
    {
      Employee director = new()
      {
        Id = Guid.NewGuid(),
        DoB = DateTime.Parse("1988-07-13"),
        LastName = "Потапчук",
        FirstName = "Алёна",
        Phone = "926-920-15-16",
        Type = EmployeeType.Director,
        Offices = new List<Office>(),
        SurveyResults = new List<SurveyResult>(),
        WorkItems = new List<WorkItem>(),
        CreatedWorkItems = new List<WorkItem>()
      };

      var myWorkItems = GenerateWorkItems(locale: Locale, count: Count_MyWorkItems, creator: director, executor: director);

      director.WorkItems =  myWorkItems;
      director.CreatedWorkItems = myWorkItems.ToList();


      Employees.Add(director);

      return director;
    }

    private static List<WorkItem> GenerateWorkItems(string locale, int count, Employee creator, Employee executor = null)
    {
      var testWorkItems = new Faker<WorkItem>(locale)
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.CreationDate, f => f.Date.Between(DateTime.Parse(common_start_day), DateTime.Parse(common_end_day)))
        .RuleFor(a => a.StartDate, f => f.Date.Between(DateTime.Parse(common_start_day), DateTime.Parse(common_end_day)))
        .RuleFor(a => a.EndDate, f => f.Date.Between(DateTime.Parse(common_start_day), DateTime.Parse(common_end_day)))
        .RuleFor(a => a.Description, f => "Description " + f.IndexFaker.ToString())
        .RuleFor(a => a.Status, f => f.PickRandom<WorkItemStatus>())
        .RuleFor(a => a.IsImportant, f => f.Random.Bool())
        .RuleFor(a => a.Creator, f => creator)
        .RuleFor(a => a.CreatorId, (f, o) => creator.Id)
        .RuleFor(a => a.Executor, (f, o) => {
          if (executor == null)
          {
            var randomExecutor = f.PickRandom(Employees);
            randomExecutor.WorkItems.Add(o);

            return randomExecutor;
          }
          else
            return executor;
          })
        .RuleFor(a => a.ExecutorId, (f, o) => o.Executor.Id)
        ;

      var newWorkItems = testWorkItems.Generate(count).ToList();
      WorkItems.AddRange(newWorkItems);

      return newWorkItems;
    }

    private static List<Client> GenerateClients(string locale, int count, Office office)
    {
      var testClients = new Faker<Client>(locale)
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.Comment, f => f.Lorem.Sentence())
        .RuleFor(a => a.DoB, f => f.Date.Between(DateTime.Parse(start_birthday), DateTime.Parse(end_birthday)))
        .RuleFor(a => a.LastName, f => f.Person.LastName)
        .RuleFor(a => a.MiddleName, f => "Отчество")
        .RuleFor(a => a.FirstName, f => f.Person.FirstName)
        .RuleFor(a => a.Phone, f => f.Person.Phone)
        .RuleFor(a => a.Sex, f => f.PickRandom<Sex>())
        .RuleFor(a => a.Office, f => office)
        .RuleFor(a => a.OfficeId, (f, o) => office.Id)
        ;

      var newClients = testClients.Generate(count).ToList();

      Clients.AddRange(newClients);

      return newClients;
    }

    private static List<Employee> GenerateManagers(string locale, int count)
    {
      var testEmployees = new Faker<Employee>(locale)
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.DoB, f => f.Date.Between(DateTime.Parse(start_birthday), DateTime.Parse(end_birthday)))
        .RuleFor(a => a.LastName, f => f.Person.LastName)
        .RuleFor(a => a.FirstName, f => f.Person.FirstName)
        .RuleFor(a => a.Phone, f => f.Person.Phone)
        .RuleFor(a => a.Type, f => EmployeeType.Manager)
        .RuleFor(a => a.Offices, (f, o) => GenerateOffices(locale, f.Random.Number(Min_count_of_office_for_manager, Max_count_of_office_for_manager), new List<Employee>() { o }))
        .RuleFor(a => a.WorkItems, (f, o) => WorkItems.Where(wi => wi.ExecutorId == o.Id).ToList())
        .RuleFor(a => a.CreatedWorkItems, (f, o) => WorkItems.Where(wi => wi.CreatorId == o.Id).ToList())
        .RuleFor(a => a.SurveyResults, f => new List<SurveyResult>())
        ;

      var newEmployeees = testEmployees.Generate(count).ToList();

      Employees.AddRange(newEmployeees);

      return newEmployeees;
    }

    private static List<Employee> GenerateOtherEmployees(string locale, int count)
    {
      var testEmployees = new Faker<Employee>(locale)
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.DoB, f => f.Date.Between(DateTime.Parse(start_birthday), DateTime.Parse(end_birthday)))
        .RuleFor(a => a.LastName, f => f.Person.LastName)
        .RuleFor(a => a.FirstName, f => f.Person.FirstName)
        .RuleFor(a => a.Phone, f => f.Person.Phone)
        .RuleFor(a => a.Type, f => EmployeeType.Employee)
        .RuleFor(a => a.Offices, (f, o) => 
        { 
          var randomOffice = f.PickRandom(Offices);
          randomOffice.Employees.Add(o);
          return new List<Office>() { randomOffice };
        } )
        .RuleFor(a => a.WorkItems, (f, o) => WorkItems.Where(wi => wi.ExecutorId == o.Id).ToList())
        .RuleFor(a => a.CreatedWorkItems, (f, o) => WorkItems.Where(wi => wi.CreatorId == o.Id).ToList())
        .RuleFor(a => a.SurveyResults, f => new List<SurveyResult>())
        ;

      var newEmployeees = testEmployees.Generate(count).ToList();

      Employees.AddRange(newEmployeees);

      return newEmployeees;
    }

    private static List<Office> GenerateOffices(string locale, int count, List<Employee> employees)
    {
      var testOffices = new Faker<Office>(locale)
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.TimeOfOpen, f => f.Date.Between(DateTime.MinValue.AddHours(Min_Hour_Of_Openning_Office), DateTime.MinValue.AddHours(Max_Hour_Of_Openning_Office)))
        .RuleFor(a => a.TimeOfClose, f => f.Date.Between(DateTime.MinValue.AddHours(Min_Hour_Of_Closing_Office), DateTime.MinValue.AddHours(Max_Hour_Of_Closing_Office)))
        .RuleFor(a => a.Phone, f => f.Person.Phone)
        .RuleFor(a => a.Email, f => f.Person.Email)
        .RuleFor(a => a.Address, f => f.Address.FullAddress())
        .RuleFor(a => a.Number, f => f.Random.Number(Min_number_of_office, Max_number_of_office))
        .RuleFor(a => a.Clients, (f, o) => GenerateClients(locale, Count_clients_per_office, o))
        .RuleFor(a => a.Employees, f => employees)
        ;

      var newOffices = testOffices.Generate(count).ToList();

      Offices.AddRange(newOffices);

      return newOffices;
    }
  }
}
