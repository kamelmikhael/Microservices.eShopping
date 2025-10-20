using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application;
using Ordering.Application.Behaviors;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            ApplicationAssemblyReference.Assembly
            , includeInternalTypes: true);

        services.AddMediatR(ApplicationAssemblyReference.Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
