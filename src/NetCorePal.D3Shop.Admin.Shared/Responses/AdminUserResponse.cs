﻿using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public class AdminUserResponse(AdminUserId id, string name, string phone, IEnumerable<string> roles)
{
    public AdminUserId Id { get; } = id;
    public string Name { get; set; } = name;
    public string Phone { get; set; } = phone;
    public IEnumerable<string> Roles { get; set; } = roles;
}