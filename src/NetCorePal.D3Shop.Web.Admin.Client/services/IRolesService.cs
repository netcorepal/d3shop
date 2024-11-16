﻿using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Web.Admin.Client.Services;

public interface IRolesService
{
    [Post("/api/Role/CreateRole")]
    Task CreateRole([Body] CreateRoleRequest request);

    [Get("/api/Role/GetAllRoles")]
    Task<ResponseData<IEnumerable<RoleResponse>>> GetAllRoles();

    [Get("/api/Role/GetRolesByCondition")]
    Task<ResponseData<IEnumerable<RoleResponse>>> GetRolesByCondition([Query] RoleQueryRequest request);

    [Get("/api/Role/GetRoleById/{id}")]
    Task<ResponseData<RoleResponse>> GetRoleById(RoleId id);

    [Get("/api/Role/GetRolePermissions/{id}")]
    Task<ResponseData<IEnumerable<RolePermissionResponse>>> GetRolePermissions(RoleId id);
}