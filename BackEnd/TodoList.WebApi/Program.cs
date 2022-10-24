using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Interfaces;
using TodoList.Domain;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TodoList.DataAccess.EFCore;
using TodoList.DataAccess.EFCore.Repositories;
using TodoList.DataAccess.EFCore.UnitOfWorks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;

var connectionStringKey = "TodoListAppConnection";
var connectionString = configuration.GetConnectionString(connectionStringKey);

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException($"Failed to find connection string with identifier: {connectionStringKey}");
}
builder.Services.AddDbContext<TodoListContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(TodoListContext).Assembly.FullName));
});

builder.Services.AddAutoMapper(typeof(DomainProfile));

#region Repositories
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<ITodoItemRepository, TodoItemRepository>();
#endregion
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddCors();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoListApp", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
