using System.Security.Claims;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpLogging;
using Promises.Api.Configs;
using Promises.Api.Configs.ColumnWriters;
using Promises.Api.Services;
using Promises.Application;
using Promises.Application.Common.Interfaces;
using Promises.Persistence;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddAuthenticationConfig(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.AddSettingsConfig(builder.Configuration);
builder.Services.AddSchedulerConfig();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddCors(options =>
    options.AddPolicy("myclients", builder =>
        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

Logger log = new LoggerConfiguration()
    .WriteTo.PostgreSQL(
        builder.Configuration.GetConnectionString("PostgreSql"),
        "Logs",
        needAutoCreateTable:true,
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            {"Message", new RenderedMessageColumnWriter()},
            {"MessageTemplate",new MessageTemplateColumnWriter()},
            {"Level", new LevelColumnWriter()},
            {"TimeStamp", new TimestampColumnWriter()},
            {"Exception", new ExceptionColumnWriter()},
            {"LogEvent", new LogEventSerializedColumnWriter()},
            {"Username", new UsernameColumnWriter()},
            {"UserId", new UserIdColumnWriter()},
            {"StatusColor", new StatusColorColumnWriter()},
            {"RequestMethod", new RequestMethodColumnWriter()}
        })
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IApplicationContext>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseSerilogRequestLogging();
app.UseHttpLogging();

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

app.UseSwagger();
app.UseSwaggerUI();
app.MigrateDatabase();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("myclients");
app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    var userId = context.User?.Identity?.IsAuthenticated != null || true
        ? context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        : null;
    int statusCode = context.Response.StatusCode;
    string statusColor = statusCode != 200 && statusCode != 201 && statusCode != 204 ? "#e74c3c" : "#2ecc71";

    LogContext.PushProperty("StatusColor", statusColor?.Replace("\"",""));
    LogContext.PushProperty("RequestMethod", context.Request.Method?.Replace("\"",""));
    LogContext.PushProperty("UserId", userId?.Replace("\"",""));
    LogContext.PushProperty("Username", username?.Replace("\"",""));

    await next();
});
app.MapControllers();
app.UseWebSockets();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.Run();