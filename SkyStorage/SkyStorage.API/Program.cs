using SkyStorage.API.Extensions;
using SkyStorage.Application.Extensions;
using SkyStorage.Domain.Entities;
using SkyStorage.Infrastructure.Extensions;
using SkyStorage.Infrastructure.MigrationTasks;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, builder.Services.AddIdentityApiEndpoints<User>());

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

var scope = app.Services.CreateScope();
var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
await migrationRunner.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("api/identity").MapIdentityApi<User>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
