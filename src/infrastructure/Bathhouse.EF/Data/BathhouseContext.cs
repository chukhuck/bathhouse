using Bathhouse.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bathhouse.EF.Data
{
  public class BathhouseContext : IdentityDbContext<Employee, Role, Guid>
  {
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Office> Offices { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<SurveyResult> SurveyResults { get; set; }
    public DbSet<WorkItem> WorkItems { get; set; }


    public BathhouseContext(DbContextOptions<BathhouseContext> options) : base(options)
    {
      Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      BuildEmployee(builder);

      BuildOffice(builder);

      BuildSurvey(builder);

      BuildSurveyResult(builder);

      BuildQuestion(builder);

      BuildClient(builder);

      BuildAnswer(builder);

      BuildWorkItem(builder);

      base.OnModelCreating(builder);
    }

    private static void BuildWorkItem(ModelBuilder builder)
    {
      builder.Entity<WorkItem>()
        .HasKey(q => q.Id);

      builder.Entity<WorkItem>()
        .Property(a => a.Description)
        .HasMaxLength(300);

      builder.Entity<WorkItem>()
        .Property(a => a.CreatorId)
        .IsRequired();
    }

    private static void BuildAnswer(ModelBuilder builder)
    {
      builder.Entity<Answer>()
        .HasKey(q => q.Id);

      builder.Entity<Answer>()
        .Property(a => a.QuestionId)
        .IsRequired();

      builder.Entity<Answer>()
        .Property(a => a.ResultId)
        .IsRequired();

      builder.Entity<Answer>()
        .Property(a => a.Value)
        .HasMaxLength(150);
    }

    private static void BuildClient(ModelBuilder builder)
    {
      builder.Entity<Client>()
        .HasKey(q => q.Id);

      builder.Entity<Client>()
        .Property(a => a.OfficeId)
        .IsRequired();

      builder.Entity<Client>()
        .Property(a => a.Comment)
        .HasMaxLength(250);

      builder.Entity<Client>()
        .Property(a => a.FirstName)
        .HasMaxLength(30);

      builder.Entity<Client>()
        .Property(a => a.LastName)
        .HasMaxLength(30);

      builder.Entity<Client>()
        .Property(a => a.MiddleName)
        .HasMaxLength(30);
    }

    private static void BuildQuestion(ModelBuilder builder)
    {
      builder.Entity<Question>()
        .HasKey(q => q.Id);

      builder.Entity<Question>()
        .HasMany(q => q.Answers)
        .WithOne(a => a.Question)
        .HasForeignKey(q => q.QuestionId);

      builder.Entity<Question>()
        .Property(a => a.SurveyId)
        .IsRequired();

      builder.Entity<Question>()
        .Property(a => a.Text)
        .HasMaxLength(300);

      builder.Entity<Question>()
        .Property(a => a.Name)
        .HasMaxLength(50);
    }

    private static void BuildSurveyResult(ModelBuilder builder)
    {
      builder.Entity<SurveyResult>()
        .HasKey(sr => sr.Id);

      builder.Entity<SurveyResult>()
        .HasMany(s => s.Answers)
        .WithOne(a => a.Result)
        .HasForeignKey(q => q.ResultId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.Entity<SurveyResult>()
        .Property(sr => sr.AuthorId)
        .IsRequired();

      builder.Entity<SurveyResult>()
        .Property(sr => sr.SurveyId)
        .IsRequired()
        ;
    }

    private static void BuildSurvey(ModelBuilder builder)
    {
      builder.Entity<Survey>()
        .HasKey(o => o.Id);

      builder.Entity<Survey>()
        .HasMany(s => s.Questions)
        .WithOne(q => q.Survey)
        .HasForeignKey(q => q.SurveyId);

      builder.Entity<Survey>()
        .HasMany(s => s.Results)
        .WithOne(sr => sr.Survey)
        .HasForeignKey(q => q.SurveyId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.Entity<Survey>()
        .Property(s => s.AuthorId)
        .IsRequired();

      builder.Entity<Survey>()
        .Property(a => a.Name)
        .HasMaxLength(50);

      builder.Entity<Survey>()
        .Property(a => a.Description)
        .HasMaxLength(300);
    }

    private static void BuildOffice(ModelBuilder builder)
    {
      builder.Entity<Office>()
        .HasKey(o => o.Id);

      builder.Entity<Office>()
        .HasMany(o => o.Clients)
        .WithOne(c => c.Office)
        .HasForeignKey(c => c.OfficeId);

      builder.Entity<Office>()
        .Property(a => a.Address)
        .HasMaxLength(150);

      builder.Entity<Office>()
        .Ignore(a => a.WorkingTimeRange);
    }

    private static void BuildEmployee(ModelBuilder builder)
    {
      builder.Entity<Employee>()
              .HasKey(e => e.Id);

      builder.Entity<Employee>()
        .HasMany(e => e.Offices)
        .WithMany(o => o.Employees);

      builder.Entity<Employee>()
        .HasMany(e => e.SurveyResults)
        .WithOne(sr => sr.Author)
        .HasForeignKey(sr => sr.AuthorId);

      builder.Entity<Employee>()
        .HasMany(e => e.Surveys)
        .WithOne(s => s.Author)
        .HasForeignKey(s => s.AuthorId);

      builder.Entity<Employee>()
        .HasMany(e => e.WorkItems)
        .WithOne(wi => wi.Creator)
        .HasForeignKey(wi => wi.CreatorId);

      builder.Entity<Employee>()
        .HasMany(e => e.CreatedWorkItems)
        .WithOne(wi => wi.Executor)
        .HasForeignKey(wi => wi.ExecutorId)
        .OnDelete(DeleteBehavior.ClientSetNull);

      builder.Entity<Employee>()
        .Property(a => a.FirstName)
        .HasMaxLength(30);

      builder.Entity<Employee>()
        .Property(a => a.LastName)
        .HasMaxLength(30);

      builder.Entity<Employee>()
        .Property(a => a.MiddleName)
        .HasMaxLength(30);

      builder.Entity<Employee>()
        .Ignore(a => a.FullName);

      builder.Entity<Employee>()
        .Ignore(a => a.ShortName);
    }
  }
}
