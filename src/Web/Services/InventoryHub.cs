using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Events;

namespace CleanArchitecture.Web.Services
{
    public class InventoryHub : Hub
    {
        // You can add methods here for client-server communication
        public async Task BroadcastInventoryUpdate(string productId, int newQuantity)
        {
            await Clients.All.SendAsync("InventoryUpdated", productId, newQuantity);
        }

        public async Task JoinProductGroup(string productId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"product_{productId}");
        }

        public async Task LeaveProductGroup(string productId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"product_{productId}");
        }

        public async Task JoinLowStockGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "low_stock_alerts");
        }

        public async Task LeaveLowStockGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "low_stock_alerts");
        }

        public async Task JoinInventoryGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "inventory_updates");
        }

        public async Task LeaveInventoryGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "inventory_updates");
        }
    }
} 