using Microsoft.EntityFrameworkCore;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure Swagger to use unique schema IDs so DTOs with the same class name but different namespaces don't collide
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type =>
    {
        // Use full name for non-generic types, replace '+' from nested types
        if (!type.IsGenericType)
            return (type.FullName ?? type.Name).Replace('+', '.');

        // For generics produce a friendly unique name: Namespace.TypeNameOfGenericArg1TypeNameOfGenericArg2...
        var genericTypeName = type.GetGenericTypeDefinition().FullName ?? type.GetGenericTypeDefinition().Name;
        genericTypeName = genericTypeName.Replace('+', '.'); // nested types fix
        // Trim the generic arity suffix like `List`1` => List
        var tickIndex = genericTypeName.IndexOf('`');
        if (tickIndex >= 0) genericTypeName = genericTypeName.Substring(0, tickIndex);

        var genericArgs = string.Join("", type.GetGenericArguments().Select(t => t.Name));
        return $"{genericTypeName}_{genericArgs}";
    });
});

builder.Services
    .AddApplicationLayer()
    .AddInfrastructureLayer(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Services.MigrateDatabase(app.Environment.IsDevelopment());
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
