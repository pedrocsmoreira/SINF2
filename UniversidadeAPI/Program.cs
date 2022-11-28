using Microsoft.EntityFrameworkCore;
using UniversidadeApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
var connection = "Data Source = UniDataBase.db";
builder.Services.AddDbContext<UniversidadeContext>(options => options.UseSqlite(connection));

builder.Services.AddCors(options => {
    options.AddPolicy("politica",
        builder => {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment()){
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("politica");

app.Run();
