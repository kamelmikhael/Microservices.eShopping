using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviors;

internal sealed class UnhandledExceptionBehavior<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger = logger 
        ?? throw new ArgumentNullException(nameof(logger));

    public async Task<TResponse> Handle(TRequest request
        , RequestHandlerDelegate<TResponse> next
        , CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex
                , "Application Request: Unhandled Exception for Request {RequestName} {@RequestAsJson}"
                , typeof(TRequest).Name
                , request);
            throw;
        }
    }
}