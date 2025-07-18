using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Order> Orders { get; }

    DbSet<Payment> Payments { get; }

    DbSet<Product> Products { get; }

    DbSet<Category> Categories { get; }

    DbSet<Customer> Customers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
