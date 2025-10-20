using Ordering.Application.Contracts.Infrastructure;

namespace Ordering.Infrastructure.Services;

internal sealed class DateTimeService : IDateTimeService
{
    public DateTime GetDate()
    {
        return DateTime.UtcNow;
    }
}