var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependenciesServices(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors();

app.UseRejectExtraFields();

app.UseAuthorization();

app.MapControllers();

app.Run();