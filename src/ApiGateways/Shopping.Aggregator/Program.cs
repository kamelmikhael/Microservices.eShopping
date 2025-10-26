using Microsoft.Extensions.Options;
using Shopping.Aggregator.Services;
using Shopping.Aggregator.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOptions<ApiSettings>()
    .BindConfiguration(nameof(ApiSettings))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<ApiSettings>>().Value);

builder.Services.AddHttpClient<ICatalogService, CatalogService>((sp, httpClient) =>
{
    ApiSettings apiSettings = sp.GetRequiredService<ApiSettings>();
    httpClient.BaseAddress = new Uri(apiSettings.CatalogUrl);
});
builder.Services.AddHttpClient<IBasketService, BasketService>((sp, httpClient) =>
{
    ApiSettings apiSettings = sp.GetRequiredService<ApiSettings>();
    httpClient.BaseAddress = new Uri(apiSettings.BasketUrl);
});
builder.Services.AddHttpClient<IOrderingService, OrderingService>((sp, httpClient) =>
{
    ApiSettings apiSettings = sp.GetRequiredService<ApiSettings>();
    httpClient.BaseAddress = new Uri(apiSettings.OrderingUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
