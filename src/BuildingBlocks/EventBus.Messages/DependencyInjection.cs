using EventBus.Messages.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MassTransit;

namespace EventBus.Messages;

public static class DependencyInjection
{
    public static IServiceCollection AddEventBus(this IServiceCollection services,
        Action<IBusRegistrationConfigurator>? busConfiguratorAction = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? busFactoryConfiguratorAction = null)
    {
        services.AddOptions<MessageBrokerSettings>()
            .BindConfiguration(nameof(MessageBrokerSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            if (busConfiguratorAction is not null) busConfiguratorAction(busConfigurator);

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();

                configurator.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                if(busFactoryConfiguratorAction is not null) busFactoryConfiguratorAction(context, configurator);
            });
        });

        return services;
    }
}
