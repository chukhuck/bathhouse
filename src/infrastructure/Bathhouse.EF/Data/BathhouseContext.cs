using Bathhouse.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bathhouse.EF.Data
{
  public class BathhouseContext : IdentityDbContext<Employee, Role, Guid>
  {
    public BathhouseContext()
    {
    }

    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Office> Offices { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Survey> Surveys { get; set; } = null!;
    public DbSet<SurveyResult> SurveyResults { get; set; } = null!;
    public DbSet<WorkItem> WorkItems { get; set; } = null!;


    public BathhouseContext(DbContextOptions<BathhouseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      BuildEmployee(builder);

      BuildIdentityEntities(builder);

      BuildOffice(builder);

      BuildQuestion(builder);

      BuildSurvey(builder);

      BuildSurveyResult(builder);

      BuildClient(builder);

      BuildAnswer(builder);

      BuildWorkItem(builder);

      BuildRoles(builder);
    }

    private static void BuildRoles(ModelBuilder builder)
    {
      builder.Entity<Role>()
        .ToTable("Roles")
        .HasData(
        new Role()
        {
          Id = Guid.NewGuid(),
          Name = Constants.AdminRoleName,
          NormalizedName = Constants.AdminRoleName.ToUpper()
        },
        new Role()
        {
          Id = Guid.NewGuid(),
          Name = Constants.DirectorRoleName,
          NormalizedName = Constants.DirectorRoleName.ToUpper()
        },
        new Role()
        {
          Id = Guid.NewGuid(),
          Name = Constants.ManagerRoleName,
          NormalizedName = Constants.ManagerRoleName.ToUpper()
        },
        new Role()
        {
          Id = Guid.NewGuid(),
          Name = Constants.EmployeeRoleName,
          NormalizedName = Constants.EmployeeRoleName.ToUpper()
        }
        ); ;
    }

    private static void BuildIdentityEntities(ModelBuilder builder)
    {
      builder.Entity<IdentityRoleClaim<Guid>>()
        .ToTable("RoleClaims");

      builder.Entity<IdentityUserRole<Guid>>()
        .ToTable("EmployeeRoles");

      builder.Entity<IdentityUserLogin<Guid>>()
        .ToTable("EmployeeLogins");

      builder.Entity<IdentityUserClaim<Guid>>()
        .ToTable("EmployeeClaims");

      builder.Entity<IdentityUserToken<Guid>>()
        .ToTable("EmployeeTokens");
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
        .IsRequired()
        ;

      builder.Entity<WorkItem>()
        .Property(a => a.ExecutorId)
        .IsRequired(false);

      builder.Entity<WorkItem>()
        .Ignore(wi => wi.IsUrgent);
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
        .Property(a => a.OfficeId);

      builder.Entity<Client>()
        .Property(a => a.Comment)
        .IsRequired(false);

      builder.Entity<Client>()
        .Property(a => a.DoB)
        .IsRequired(false);

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

      builder.Entity<Client>()
        .Property(a => a.Phone)
        .HasMaxLength(20);
    }

    private static void BuildQuestion(ModelBuilder builder)
    {
      builder.Entity<Question>()
        .HasKey(q => q.Id);

      builder.Entity<Question>()
        .HasMany(q => q.Answers)
        .WithOne(a => a.Question)
        .HasForeignKey(q => q.QuestionId)
        .OnDelete(DeleteBehavior.NoAction);

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
        .HasForeignKey(q => q.ResultId);

      builder.Entity<SurveyResult>()
        .Property(sr => sr.AuthorId);

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
        .Property(a => a.Description)
        .IsRequired(false);

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

#pragma warning disable CS8603
      builder.Entity<Office>()
        .HasMany(o => o.Clients)
        .WithOne(c => c.Office)
        .HasForeignKey(c => c.OfficeId)
        .OnDelete(DeleteBehavior.SetNull);
#pragma warning restore CS8603

      builder.Entity<Office>()
        .Property(a => a.Email)
        .IsRequired(false);

      builder.Entity<Office>()
        .Property(a => a.Address)
        .IsRequired(false);

      builder.Entity<Office>()
        .Property(a => a.Phone)
        .IsRequired(false);

      builder.Entity<Office>()
        .Property(a => a.Address)
        .HasMaxLength(200);

      builder.Entity<Office>()
        .Property(a => a.Phone)
        .HasMaxLength(20);

      builder.Entity<Office>()
        .Property(a => a.Email)
        .HasMaxLength(80);

      builder.Entity<Office>()
        .Ignore(a => a.WorkingTimeRange);
    }

    private static void BuildEmployee(ModelBuilder builder)
    {
      builder.Entity<Employee>()
        .ToTable("Employees");

      builder.Entity<Employee>()
        .HasMany(e => e.Offices)
        .WithMany(o => o.Employees);

#pragma warning disable CS8603
      builder.Entity<Employee>()
        .HasMany(e => e.SurveyResults)
        .WithOne(sr => sr.Author)
        .HasForeignKey(sr => sr.AuthorId)
        .OnDelete(DeleteBehavior.SetNull);
#pragma warning restore CS8603

      builder.Entity<Employee>()
        .HasMany(e => e.Surveys)
        .WithOne(s => s.Author)
        .HasForeignKey(s => s.AuthorId);

      builder.Entity<Employee>()
        .HasMany(e => e.CreatedWorkItems)
        .WithOne(wi => wi.Creator)
        .HasForeignKey(wi => wi.CreatorId);

      // TODO .OnDelete(DeleteBehavior.SetNull)
#pragma warning disable CS8603
      builder.Entity<Employee>()
        .HasMany(e => e.WorkItems)
        .WithOne(wi => wi.Executor)
        .HasForeignKey(wi => wi.ExecutorId)
        .OnDelete(DeleteBehavior.Restrict);
#pragma warning restore CS8603

      builder.Entity<Employee>()
        .Property(a => a.DoB)
        .IsRequired(false);

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
