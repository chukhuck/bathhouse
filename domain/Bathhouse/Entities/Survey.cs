using Bathhouse.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Entities
{
  public class Survey : Entity
  {
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(50, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 50 symbols.")]
    public string Name { get; set; } = "Новый опрос";

    [DataType(DataType.Text)]
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    public string Description { get; set; } = "Описание нового опроса";

    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    public ICollection<Question> Questions { get; set; }

    public ICollection<SurveyResult> Results { get; set; }




    /// <summary>
    /// Get All of results of this survey
    /// </summary>
    /// <returns>Result</returns>
    public virtual SurveyResultModel GetResult()
    {
      SurveyResultModel result = new SurveyResultModel();

      result.Id = Id;
      result.Name = Name;

      result.Headers = GetHeaders();
      result.Data = GetData();

      return result;
    }

    /// <summary>
    /// Get all of results of this survey as List
    /// </summary>
    /// <returns></returns>
    private List<List<string>> GetData() => Results.Select(r => r.ToList()).ToList();

    /// <summary>
    /// Get headrers for this survey
    /// </summary>
    /// <returns>List of headers</returns>
    private List<SurveyResultHeader> GetHeaders()
    {
      List<SurveyResultHeader> headers = new();

      headers.Add(new SurveyResultHeader() { Type = SurveyResultHeaderType.Datetime, Text = "Date" });
      headers.Add(new SurveyResultHeader() { Type = SurveyResultHeaderType.Text, Text = "Employee" });
      headers.Add(new SurveyResultHeader() { Type = SurveyResultHeaderType.Text, Text = "Office" });

      headers.AddRange(Questions.Select(q => new SurveyResultHeader() { Type = SurveyResultHeaderType.Text, Text = q.Name }));

      return headers;
    }
  }
}
