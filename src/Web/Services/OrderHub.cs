using Microsoft.AspNetCore.SignalR;
using CleanArchitecture.Domain.Events;
using CleanArchitecture.Application.Orders.Common;

namespace CleanArchitecture.Web.Services;

public class OrderHub : Hub
{
    public async Task JoinOrderGroup(string orderId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"order_{orderId}");
    }

    public async Task LeaveOrderGroup(string orderId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"order_{orderId}");
    }

    public async Task JoinCustomerGroup(string customerId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"customer_{customerId}");
    }

    public async Task LeaveCustomerGroup(string customerId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"customer_{customerId}");
    }

    public async Task JoinAdminGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "admins");
    }

    public async Task LeaveAdminGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admins");
    }
} 