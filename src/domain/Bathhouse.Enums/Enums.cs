using System.ComponentModel.DataAnnotations;

namespace Bathhouse.ValueTypes
{
  /// <summary>
  /// Type of data in column
  /// </summary>
  public enum DataType
  {
    [Display(Name= "Текст")]
    Text,
    [Display(Name="Целое")]
    Number,
    [Display(Name="Дробное")]
    Decimal,
    [Display(Name="Дата")]
    DateTime,
    [Display(Name="ПравдаЛожь")]
    Bool,
    [Display(Name="Фото")]
    Photo
  }

  /// <summary>
  /// Gender of person
  /// </summary>
  public enum Sex
  {
    [Display(Name="М")]
    Male,
    [Display(Name="Ж")]
    Female,
    [Display(Name="Неизвестно")]
    Unknown
  }

  /// <summary>
  /// Type of employee
  /// </summary>
  public enum EmployeeType
  {
    [Display(Name="Директор бани")]
    Director,
    [Display(Name="Менеджер")]
    Manager,
    [Display(Name="Сотрудник")]
    Employee,
    [Display(Name="Техническая поддержка")]
    TechnicalSupport
  }

  /// <summary>
  /// Type of question
  /// </summary>
  public enum QuestionType
  {
    [Display(Name="Целое")]
    Number,
    [Display(Name="Дробное")]
    Decimal,
    [Display(Name="ДаНет")]
    YesNo,
    [Display(Name="Текст")]
    Text,
    [Display(Name="Фото")]
    Photo,
    [Display(Name="Дата")]
    DateTime
  }

  /// <summary>
  /// Type of generating survey summary
  /// </summary>
  public enum SurveyResultSummaryType
  {
    [Display(Name="Простые")]
    Simple,
    [Display(Name="С агрегацией")]
    Aggregated
  }

  /// <summary>
  /// Status of survey
  /// </summary>
  public enum SurveyStatus
  {
    [Display(Name="В работе")]
    Work,
    [Display(Name="Архивный")]
    Archive,
    [Display(Name="Удален")]
    Deleted
  }

  /// <summary>
  /// Status of WorkItem
  /// </summary>
  public enum WorkItemStatus
  {
    [Display(Name="Создан")]
    Created,
    [Display(Name="В работе")]
    InWork,
    [Display(Name="Выполнено")]
    Done,
    [Display(Name="Отменено")]
    Canceled
  }
}
