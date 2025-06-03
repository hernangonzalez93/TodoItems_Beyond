using TodoItems_Beyond.Business;
using TodoItems_Beyond.Contracts;
using TodoItems_Beyond.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Inyectar dependencias 
builder.Services.AddSingleton<ITodoListRepository, TodoListRepository>();
builder.Services.AddSingleton<ITodoList, TodoList>();
builder.Services.AddSingleton<ItemManagement, ItemManagement>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
