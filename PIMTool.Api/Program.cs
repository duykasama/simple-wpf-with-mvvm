using Microsoft.AspNetCore.Localization;
using PIMTool.Api;
using PIMTool.Api.Constants;
using PIMTool.Api.Middlewares;
using Serilog;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(cfg => cfg.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterNHibernate();
builder.Services.RegisterServices();
builder.Services.RegisterAutoMapper();

builder.Services.AddLocalization();
builder.AddSerilog();

builder.Services
    .AddExceptionHandler<GlobalExceptionHandlerMiddleware>()
    .AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(AppCultures.English),
    SupportedCultures = [
        new CultureInfo(AppCultures.English),
        new CultureInfo(AppCultures.French),
    ],
    SupportedUICultures = [
        new CultureInfo(AppCultures.English),
        new CultureInfo(AppCultures.French),
    ],
});

app.MapControllers();

app.Run();

Log.CloseAndFlush();
