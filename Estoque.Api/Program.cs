using Estoque.Api.Data;
using Estoque.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do Banco de Dados
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// 2. Configuração do CORS (Corrigido: faltava "policy =>")
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd", policy => 
    {
        policy.WithOrigins("http://localhost:3000") // Porta do FrontEnd
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Controllers e Configuração de Comportamento da API (Movido para cá)
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
        options.InvalidModelStateResponseFactory = ctx => 
            new UnprocessableEntityObjectResult(ctx.ModelState);
    });

// 4. Registro de Filtros
builder.Services.AddScoped<ValidateModelAttribute>();

var app = builder.Build();

// --- Pipeline de Requisição ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontEnd"); // Corrigido: Faltava ponto e vírgula aqui
app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }