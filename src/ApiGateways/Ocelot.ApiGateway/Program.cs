using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
      .SetBasePath(builder.Environment.ContentRootPath)
      .AddOcelot(
        $"ocelot.{builder.Environment.EnvironmentName}.json"
        , optional: false
        , reloadOnChange: true);

builder.Services
    .AddOcelot(builder.Configuration);

builder
    .Logging
    .AddConsole()
    .AddDebug();

var app = builder.Build();

await app.UseOcelot();

await app.RunAsync();
