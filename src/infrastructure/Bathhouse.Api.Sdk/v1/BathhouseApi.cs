using Bathhouse.Contracts.Models.v1.Requests;
using Bathhouse.Contracts.Models.v1.Responses;
using Bathhouse.Contracts.v1.Models.Queries;
using Bathhouse.ValueTypes;
using Chuk.Helpers.AspNetCore;
using Refit;
using System;
using System.Threading.Tasks;

namespace Bathhouse.Api.v1.Sdk
{
  interface IBathhouseApi
  {
    [Get("/Workitems")]
    Task<PaginatedResponse<WorkItemResponse>> GetAllWorkItems(
      [Body]PaginationQuery paginationQuery,
      [Body]WorkItemFilterQuery filter);
    
    [Get("/Workitems/{workItemId:guid}")]
    Task<WorkItemResponse> GetWorkItemById(Guid workItemId);

    [Post("/Workitems")]
    Task<WorkItemResponse> CreateWorkItem([Body]WorkItemRequest request);

    [Put("/Workitems/{workItemId:guid}")]
    Task UpdateWorkItem(Guid workItemId, [Body]WorkItemRequest request);

    [Delete("/Workitems/{workItemId:guid}")]
    Task DeleteWorkItem(Guid workItemId);

    [Post("/Workitems/{employeeId:guid}/workitems/{workitemId:guid}/status")]
    Task ChangeStatusOfWorkItem(Guid workitemId, WorkItemStatus newWorkItemStatus);
  }
}
