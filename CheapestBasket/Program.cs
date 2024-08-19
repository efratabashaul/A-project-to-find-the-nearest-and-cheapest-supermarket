using AutoMapper;
using CheapestBasket.Repository;
using CheapestBasket.Service;
using Microsoft.AspNetCore.Hosting;
//using MockContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.myAddServices();
//builder.Services.AddSingleton<IContext, Data>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MapperProfile)); // Replace Startup with your class name

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();