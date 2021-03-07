using Bathhouse.Contracts;
using Bathhouse.Contracts.Models.v1.Requests;
using Bathhouse.Contracts.Models.v1.Responses;
using Bathhouse.Contracts.v1.Models.Queries;
using Bathhouse.ValueTypes;
using Chuk.Helpers.AspNetCore;
using Refit;
using System;
using System.Threading.Tasks;

namespace Bathhouse.Api.Sdk.v1
{
  interface IBathhouseApi
  {
    [Get(ApiRoutes.GetAllWorkItems)]
    Task<PaginatedResponse<WorkItemResponse>> GetAllWorkItemsAsync(
      [Body]PaginationQuery paginationQuery,
      [Body]WorkItemFilterQuery filter);
    
    [Get(ApiRoutes.GetWorkItemById)]
    Task<WorkItemResponse> GetWorkItemByIdAsync(Guid workItemId);

    [Post(ApiRoutes.CreateWorkItem)]
    Task<WorkItemResponse> CreateWorkItemAsync([Body]WorkItemRequest request);

    [Put(ApiRoutes.UpdateWorkItem)]
    Task UpdateWorkItemAsync(Guid workItemId, [Body]WorkItemRequest request);

    [Delete(ApiRoutes.DeleteWorkItem)]
    Task DeleteWorkItemAsync(Guid workItemId);

    [Post(ApiRoutes.ChangeStatusOfWorkItem)]
    Task ChangeStatusOfWorkItemAsync(Guid workitemId, WorkItemStatus newWorkItemStatus);
  }
}
