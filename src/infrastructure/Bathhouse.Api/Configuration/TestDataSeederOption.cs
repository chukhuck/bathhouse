using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bathhouse.Api.Configuration
{
  public class TestDataSeederOption
  {
    public const string Position = "TestDataSeederOption";

    public int Count_of_managers { get; set; }
    public int Count_Of_Eemployees { get; set; }
    public string Locale { get; set; } = "ru";
    public int Count_Of_WorkItem { get; set; }
    public int Count_Of_Surveys { get; set; }
    public int Count_Of_SurveyResult { get; set; }
    public int Min_Count_Of_Question_In_Survey { get; set; }
    public int Max_Count_Of_Question_In_Survey { get; set; }
    public int Min_Number_In_Answer { get; set; }
    public int Max_Number_In_Answer { get; set; }
    public int Count_MyWorkItems { get; set; }
    public int Min_count_of_office_for_manager { get; set; }
    public int Max_count_of_office_for_manager { get; set; }
    public int Count_clients_per_office { get; set; }
    public int Min_number_of_office { get; set; }
    public int Max_number_of_office { get; set; }
    public string Common_start_day { get; set; } = "2020-01-01";
    public string Common_end_day { get; set; } = "2021-01-07";
    public string Start_birthday { get; set; } = "1945-01-01";
    public string End_birthday { get; set; } = "2002-01-01";
    public int Min_Hour_Of_Openning_Office { get; set; }
    public int Max_Hour_Of_Openning_Office { get; set; }
    public int Min_Hour_Of_Closing_Office { get; set; }
    public int Max_Hour_Of_Closing_Office { get; set; }
  }
}
