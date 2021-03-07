using Bathhouse.Contracts;
using Bathhouse.Contracts.Models.Requests.v1;
using Bathhouse.Contracts.Models.Responses.v1;
using Bathhouse.Contracts.Models.Queries.v1;
using Bathhouse.ValueTypes;
using Chuk.Helpers.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bathhouse.Api.Sdk.v1
{
  interface IBathhouseApi
  {
    #region Client
    [Get(ApiRoutes.GetAllClients)]
    Task<PaginatedResponse<WorkItemResponse>> GetAllClientsAsync(
      [Query] PaginationQuery paginationQuery,
      [Query] Guid officeId);

    [Get(ApiRoutes.GetClientById)]
    Task<ClientResponse> GetClientByIdAsync(Guid clientId);

    [Post(ApiRoutes.CreateClient)]
    Task<ClientResponse> CreateClientAsync([Body] ClientRequest request);

    [Put(ApiRoutes.UpdateClient)]
    Task UpdateClientAsync(Guid clientId, [Body] ClientRequest request);

    [Delete(ApiRoutes.DeleteClient)]
    Task DeleteClientAsync(Guid clientId);

    #endregion

    #region Employee
    [Get(ApiRoutes.GetAllEmployees)]
    public Task<PaginatedResponse<EmployeeResponse>> GetAllEmployeesAsync([Query] PaginationQuery paginationQuery);

    [Get(ApiRoutes.GetEmployeeById)]
    public Task<EmployeeResponse> GetEmployeeByIdAsync(Guid employeeId);

    [Post(ApiRoutes.CreateEmployee)]
    public Task<EmployeeResponse> CreateEmployeeAsync([Body] EmployeeRequest request);

    [Put(ApiRoutes.UpdateEmployee)]
    public Task UpdateEmployeeAsync(Guid employeeId, [Body] EmployeeRequest request);

    [Delete(ApiRoutes.DeleteEmployee)]
    public Task DeleteEmployeeAsync(Guid employeeId);

    [Get(ApiRoutes.GetRolesForEmployee)]
    public Task<string> GetRolesForEmployeeAsync(Guid employeeId);

    [Post(ApiRoutes.AddRoleForEmployee)]
    public Task<IdentityResult> AddRoleForEmployeeAsync(Guid employeeId, string newRole);

    [Delete(ApiRoutes.DeleteRoleFromEmployee)]
    public Task DeleteRoleFromEmployeeAsync(Guid employeeId, string newRole);

    [Get(ApiRoutes.GetAllTheDirectors)]
    public Task<EmployeeResponse> GetAllTheDirectorsAsync();

    [Get(ApiRoutes.GetAllSimpleEmployees)]
    public Task<EmployeeResponse> GetAllSimpleEmployeesAsync();

    [Get(ApiRoutes.GetAllManagers)]
    public Task<EmployeeResponse> GetAllManagersAsync();

    [Get(ApiRoutes.GetAllTechSupporters)]
    public Task<EmployeeResponse> GetAllTechSupportersAsync();

    [Get(ApiRoutes.GetOfficesForEmployee)]
    public Task<OfficeResponse> GetOfficesForEmployeeAsync(Guid employeeId);

    [Delete(ApiRoutes.DeleteOfficeFromEmployee)]
    public Task DeleteOfficeFromEmployeeAsync(Guid employeeId, Guid officeId);

    [Post(ApiRoutes.AddOfficeToEmployee)]
    public Task<IEnumerable<OfficeResponse>> AddOfficeToEmployeeAsync(Guid employeeId, Guid officeId);

    [Put(ApiRoutes.SetOfficesForEmployee)]
    public Task<IEnumerable<OfficeResponse>> SetOfficesForEmployeeAsync(
      Guid employeeId, 
      [Body] IEnumerable<Guid> officeIds);

    [Get(ApiRoutes.GetWorkItemsForEmployee)]
    public Task<WorkItemResponse> GetWorkItemsForEmployeeAsync(Guid employeeId);

    [Get(ApiRoutes.GetAllWorkItemsCreatedByEmployee)]
    public Task<IEnumerable<WorkItemResponse>> GetAllWorkItemsCreatedByEmployeeAsync(Guid employeeId);

    [Get(ApiRoutes.GetWorkItemCreatedByEmployee)]
    public Task<WorkItemResponse> GetWorkItemCreatedByEmployeeAsync(Guid employeeId, Guid workitemId);

    [Delete(ApiRoutes.DeleteWorkItemCreatedByEmployee)]
    public Task DeleteWorkItemCreatedByEmployeeAsync(Guid employeeId, Guid workitemId);

    [Post(ApiRoutes.CreateWorkItemByEmployee)]
    public Task<WorkItemResponse> CreateWorkItemByEmployeeAsync(Guid employeeId, [Body] WorkItemRequest workItem);

    [Put(ApiRoutes.UpdateCreatedWorkItem)]
    public Task UpdateCreatedWorkItemAsync(Guid employeeId, Guid workitemId, [Body] WorkItemRequest request);

    [Put(ApiRoutes.ChangeStatusMyWorkItem)]
    public Task ChangeStatusMyWorkItemAsync(Guid employeeId, Guid workitemId, WorkItemStatus newWorkItemStatus);

    [Get(ApiRoutes.GetAllSurveysForEmployee)]
    public Task<IEnumerable<SurveyResponse>> GetAllSurveysForEmployeeAsync(Guid employeeId);

    [Get(ApiRoutes.GetSurveySummaryForEmployee)]
    public Task<SurveySummaryResponse> GetSurveySummaryForEmployeeAsync(
      Guid employeeId, 
      Guid surveyId, 
      SurveyResultSummaryType summarytype);

    [Delete(ApiRoutes.DeleteSurveyForEmployee)]
    public Task DeleteSurveyForEmployeeAsync(Guid employeeId, Guid surveyId);

    [Get(ApiRoutes.GetSurveyForEmployee)]
    public Task<SurveyResponse> GetSurveyForEmployeeAsync(Guid employeeId, Guid surveyId);

    [Post(ApiRoutes.CreateSurveyForEmployee)]
    public Task<SurveyResponse> CreateSurveyForEmployeeAsync(Guid employeeId, [Body] SurveyRequest survey);

    [Put(ApiRoutes.UpdateSurveyForEmployee)]
    public Task UpdateSurveyForEmployeeAsync(
      Guid employeeId,
      Guid surveyId,
      string? newName,
      string? newDescription);
    #endregion


    #region Office
    [Get(ApiRoutes.GetAllOffices)]
    Task<PaginatedResponse<OfficeResponse>> GetAllOfficesAsync(
      [Query] PaginationQuery paginationQuery,
      [Query] int? officeNumber = null);

    [Get(ApiRoutes.GetOfficeById)]
    Task<OfficeResponse> GetOfficeByIdAsync(Guid officeId);

    [Post(ApiRoutes.CreateOffice)]
    Task<OfficeResponse> CreateOfficeAsync([Body] OfficeRequest request);

    [Put(ApiRoutes.UpdateOffice)]
    Task UpdateOfficeAsync(Guid officeId, [Body] OfficeRequest request);

    [Delete(ApiRoutes.DeleteOffice)]
    Task DeleteOfficeAsync(Guid officeId);

    [Put(ApiRoutes.GetManagersInOffice)]
    public Task<EmployeeResponse> GetManagersInOfficeAsync(Guid officeId);

    [Put(ApiRoutes.GetEmployeesInOffice)]
    public Task<EmployeeResponse> GetEmployeesInOfficeAsync(Guid officeId);

    [Put(ApiRoutes.DeleteEmployeeFromOffice)]
    public Task DeleteEmployeeFromOfficeAsync(Guid officeId, Guid employeeId);

    [Put(ApiRoutes.AddEmployeeToOffice)]
    public Task<IEnumerable<EmployeeResponse>> AddEmployeeToOfficeAsync(Guid officeId, Guid employeeId);

    [Put(ApiRoutes.SetEmployeesToOffice)]
    public Task<IEnumerable<EmployeeResponse>> SetEmployeesToOfficeAsync(
      Guid officeId,[Body] IEnumerable<Guid> employeeIds);
    #endregion


    #region Question
    [Get(ApiRoutes.GetQuestionById)]
    Task<QuestionResponse> GetQuestionByIdAsync(Guid questionId);

    [Post(ApiRoutes.CreateQuestion)]
    Task<QuestionResponse> CreateQuestionAsync([Body] QuestionRequest request);

    [Put(ApiRoutes.UpdateQuestion)]
    Task UpdateQuestionAsync(Guid questionId, [Body] QuestionRequest request);

    [Delete(ApiRoutes.DeleteQuestion)]
    Task DeleteQuestionAsync(Guid questionId);
    #endregion


    #region Role
    [Get(ApiRoutes.GetAllRoles)]
    Task<PaginatedResponse<RoleResponse>> GetAllRolesAsync();

    [Get(ApiRoutes.GetRoleById)]
    Task<RoleResponse> GetRoleByIdAsync(Guid roleId);

    [Post(ApiRoutes.CreateRole)]
    Task<RoleResponse> CreateRoleAsync(string name);

    [Put(ApiRoutes.UpdateRole)]
    Task UpdateRoleAsync(Guid roleId, string newName);

    [Delete(ApiRoutes.DeleteRole)]
    Task DeleteRoleAsync(Guid roleId);

    [Get(ApiRoutes.GetEmployeesInRole)]
    public Task<IEnumerable<EmployeeResponse>> GetEmployeesInRoleAsync(Guid roleId);

    [Put(ApiRoutes.AddEmployeeToRole)]
    public Task AddEmployeeToRoleAsync(Guid roleId, Guid employeeId);

    [Delete(ApiRoutes.DeleteEmployeeFromRole)]
    public Task DeleteEmployeeFromRoleAsync(Guid roleId, Guid employeeId);

    #endregion


    #region Survey
    [Get(ApiRoutes.GetAllSurveys)]
    Task<PaginatedResponse<SurveyResponse>> GetAllSurveysAsync(
      [Query] PaginationQuery paginationQuery,
      [Query] SurveyFilterQuery filter);

    [Get(ApiRoutes.GetSurveyById)]
    Task<SurveyResponse> GetSurveyByIdAsync(Guid surveyId);

    [Post(ApiRoutes.CreateSurvey)]
    Task<SurveyResponse> CreateSurveyAsync([Body] SurveyRequest request);

    [Put(ApiRoutes.UpdateSurvey)]
    Task UpdateSurveyAsync(Guid surveyId, [Body] SurveyRequest request);

    [Delete(ApiRoutes.DeleteSurvey)]
    Task DeleteSurveyAsync(Guid surveyId);

    [Get(ApiRoutes.GetAllQuestionsInSurvey)]
    public Task<PaginatedResponse<SurveyResponse>> GetAllQuestionsInSurveyAsync(Guid surveyId);

    [Get(ApiRoutes.GetQuestionInSurvey)]
    public Task<QuestionResponse> GetQuestionInSurveyAsync(Guid surveyId, Guid questionId);

    [Post(ApiRoutes.AddQuestionToSurvey)]
    public Task<QuestionResponse> AddQuestionToSurveyAsync(Guid surveyId, [Body] QuestionRequest request);

    [Put(ApiRoutes.UpdateQuestionInSurvey)]
    public Task UpdateQuestionInSurveyAsync(Guid surveyId, Guid questionId, [Body] QuestionRequest request);

    [Delete(ApiRoutes.DeleteQuestionFromSurvey)]
    public Task DeleteQuestionFromSurveyAsync(Guid surveyId, Guid questionId);

    [Get(ApiRoutes.GetSurveySummary)]
    public Task<SurveySummaryResponse> GetSurveySummaryAsync(Guid surveyId, [Query] SurveyResultSummaryType summaryType);


#endregion


    #region WorkItem

    [Get(ApiRoutes.GetAllWorkItems)]
    Task<PaginatedResponse<WorkItemResponse>> GetAllWorkItemsAsync(
      [Query] PaginationQuery paginationQuery,
      [Query] WorkItemFilterQuery filter);

    [Get(ApiRoutes.GetWorkItemById)]
    Task<WorkItemResponse> GetWorkItemByIdAsync(Guid workItemId);

    [Post(ApiRoutes.CreateWorkItem)]
    Task<WorkItemResponse> CreateWorkItemAsync([Body] WorkItemRequest request);

    [Put(ApiRoutes.UpdateWorkItem)]
    Task UpdateWorkItemAsync(Guid workItemId, [Body] WorkItemRequest request);

    [Delete(ApiRoutes.DeleteWorkItem)]
    Task DeleteWorkItemAsync(Guid workItemId);

    [Post(ApiRoutes.ChangeStatusOfWorkItem)]
    Task ChangeStatusOfWorkItemAsync(Guid workitemId, WorkItemStatus newWorkItemStatus);

    #endregion
  }
}
