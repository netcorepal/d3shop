﻿using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using System.ComponentModel.DataAnnotations;

namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public class CreateDepartmentRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DeptId? ParentId { get; set; } 
}