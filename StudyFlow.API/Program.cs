var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependenciesServices(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseHangfireDashboard("/Jobs",new DashboardOptions
{
    Authorization =
    [
       new  HangfireCustomBasicAuthenticationFilter
       {
           User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
           Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
       }
    ],
    DashboardTitle = "Study Flow Dashboard"
});

app.UseCors();
app.MapHealthChecks("health",new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRejectExtraFields();

app.UseAuthorization();

app.MapControllers();

app.Run();