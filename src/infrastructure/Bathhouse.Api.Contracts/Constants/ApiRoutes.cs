namespace Bathhouse.Api.Contracts
{
  public class ApiRoutes
  {
    #region WorkItem
    public const string GetAllWorkItems = "/workitems";
    public const string GetWorkItemById = "/workitems/{workItemId:guid}";
    public const string CreateWorkItem = "/workitems/";
    public const string UpdateWorkItem = "/workitems/{workItemId:guid}";
    public const string DeleteWorkItem = "/workitems/{workItemId:guid}";
    public const string ChangeStatusOfWorkItem = "/workitems/{workitemId:guid}/status";

    #endregion

    #region Client
    public const string GetAllClients = "/clients";
    public const string GetClientById = "/clients/{clientId:guid}";
    public const string CreateClient = "/clients";
    public const string UpdateClient = "/clients/{clientId:guid}";
    public const string DeleteClient = "/clients/{clientId:guid}";

    #endregion

    #region Employee
    public const string GetAllEmployees = "/empoyees";
    public const string GetEmployeeById = "/empoyees/{empoyeeId:guid}";
    public const string CreateEmployee = "/empoyees";
    public const string UpdateEmployee = "/empoyees/{empoyeeId:guid}";
    public const string DeleteEmployee = "/empoyees/{empoyeeId:guid}";
    public const string GetRolesForEmployee = "/empoyees/{employeeId:guid}/roles";
    public const string AddRoleForEmployee = "/empoyees/{employeeId:guid}/roles";
    public const string DeleteRoleFromEmployee = "/empoyees/{employeeId:guid}/roles";
    public const string GetAllTheDirectors = "/empoyees/directors";
    public const string GetAllSimpleEmployees = "/empoyees/employees";
    public const string GetAllManagers = "/empoyees/managers";
    public const string GetAllTechSupporters = "/empoyees/techsupporters";
    public const string GetOfficesForEmployee = "/empoyees/{employeeId:guid}/offices";
    public const string DeleteOfficeFromEmployee = "/empoyees/{employeeId:guid}/offices/{officeId:guid}";
    public const string AddOfficeToEmployee = "/empoyees/{employeeId:guid}/offices";
    public const string SetOfficesForEmployee = "/empoyees/{employeeId:guid}/offices";
    public const string GetWorkItemsForEmployee = "/empoyees/{employeeId:guid}/myworkitems";
    public const string GetAllWorkItemsCreatedByEmployee = "/empoyees/{employeeId:guid}/workitems";
    public const string GetWorkItemCreatedByEmployee = "/empoyees/{employeeId:guid}/workitems/{workitemId:guid}";
    public const string DeleteWorkItemCreatedByEmployee = "/empoyees/{employeeId:guid}/workitems/{workitemId:guid}";
    public const string CreateWorkItemByEmployee = "/empoyees/{employeeId:guid}/workitems";
    public const string UpdateCreatedWorkItem = "/empoyees/{employeeId:guid}/workitems/{workitemId:guid}";
    public const string ChangeStatusMyWorkItem = "/empoyees/{employeeId:guid}/workitems/{workitemId:guid}/status";
    public const string GetAllSurveysForEmployee = "/empoyees/{employeeId:guid}/surveys";
    public const string GetSurveySummaryForEmployee = "/empoyees/{employeeId:guid}/surveys/{surveyId:guid}/summary";
    public const string DeleteSurveyForEmployee = "/empoyees/{employeeId:guid}/surveys/{surveyId:guid}";
    public const string GetSurveyForEmployee = "/empoyees/{employeeId:guid}/surveys/{surveyId:guid}";
    public const string CreateSurveyForEmployee = "/empoyees/{employeeId:guid}/surveys";
    public const string UpdateSurveyForEmployee = "/empoyees/{employeeId:guid}/surveys/{surveyId:guid}";


    #endregion

    #region Office
    public const string GetAllOffices = "/offices";
    public const string GetOfficeById = "/offices/{officeId:guid}";
    public const string CreateOffice = "/offices";
    public const string UpdateOffice = "/offices/{officeId:guid}";
    public const string DeleteOffice = "/offices/{officeId:guid}";
    public const string GetManagersInOffice = "/offices/{officeId:guid}/managers";
    public const string GetEmployeesInOffice = "/offices/{officeId:guid}/employees";
    public const string DeleteEmployeeFromOffice = "/offices/{officeId:guid}/employees/{employeeId:guid}";
    public const string AddEmployeeToOffice = "/offices/{officeId:guid}/employees";
    public const string SetEmployeesToOffice = "/offices/{officeId:guid}/employees";

    #endregion

    #region Question
    public const string GetQuestionById = "/questions/{questionId:guid}";
    public const string CreateQuestion = "/questions";
    public const string UpdateQuestion = "/questions/{questionId:guid}";
    public const string DeleteQuestion = "/questions/{questionId:guid}";

    #endregion

    #region Role
    public const string GetAllRoles = "/roles";
    public const string GetRoleById = "/roles/{roleId:guid}";
    public const string CreateRole = "/roles";
    public const string UpdateRole = "/roles/{roleId:guid}";
    public const string DeleteRole = "/roles/{roleId:guid}";
    public const string GetEmployeesInRole = "/roles/{roleId:guid}/employees";
    public const string AddEmployeeToRole = "/roles/{roleId:guid}/employees";
    public const string DeleteEmployeeFromRole = "/roles/{roleId:guid}/employees/{employeeId:guid}";

    #endregion

    #region Survey
    public const string GetAllSurveys = "/surveys";
    public const string GetSurveyById = "/surveys/{surveyId:guid}";
    public const string CreateSurvey = "/surveys";
    public const string UpdateSurvey = "/surveys/{surveyId:guid}";
    public const string DeleteSurvey = "/surveys/{surveyId:guid}";
    public const string GetAllQuestionsInSurvey = "/surveys/{surveyId:guid}/questions";
    public const string GetQuestionInSurvey = "/surveys/{surveyId:guid}/questions/{questionId:guid}";
    public const string AddQuestionToSurvey = "/surveys/{surveyId:guid}/questions";
    public const string UpdateQuestionInSurvey = "/surveys/{surveyId:guid}/questions/{questionId:guid}";
    public const string DeleteQuestionFromSurvey = "/surveys/{surveyId:guid}/questions/{questionId:guid}";
    public const string GetSurveySummary = "/surveys/{surveyId:guid}/summary";

    #endregion
  }
}
