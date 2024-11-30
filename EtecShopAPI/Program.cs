using EtecShopAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string conexao = builder.Configuration.GetConnectionString("EtecShopDB");
var servidor = ServerVersion.AutoDetect(conexao);

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySql(conexao, servidor)
);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
