using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
      .SetBasePath(builder.Environment.ContentRootPath)
      .AddOcelot(
        $"ocelot.{builder.Environment.EnvironmentName}.json"
        , optional: false
        , reloadOnChange: true);

builder.Services
    .AddOcelot(builder.Configuration)
    .AddCacheManager(settings => settings.WithDictionaryHandle());

builder
    .Logging
    .AddConsole()
    .AddDebug();

var app = builder.Build();

await app.UseOcelot();

await app.RunAsync();
