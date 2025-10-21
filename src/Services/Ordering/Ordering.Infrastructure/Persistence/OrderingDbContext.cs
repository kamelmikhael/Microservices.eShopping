using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Orders;
using Ordering.SharedKernel;

namespace Ordering.Infrastructure.Persistence;

public class OrderingDbContext(
    DbContextOptions<OrderingDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _ = ChangeTracker.Entries<EntityBase>()
            .Select(entry =>
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "Admin";
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedAt = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = "Admin";
                }

                return entry.Entity;
            }).ToList();

        return base.SaveChangesAsync(cancellationToken);
    }
}