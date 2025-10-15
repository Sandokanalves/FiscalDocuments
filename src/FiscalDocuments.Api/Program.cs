using FiscalDocuments.Domain.Interfaces;
using FiscalDocuments.Infrastructure.Persistence.Context;
using FiscalDocuments.Infrastructure.Persistence.Repositories;
using System.Reflection;
using FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(UploadFiscalDocumentCommand).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(UploadFiscalDocumentCommand).Assembly);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IFiscalDocumentRepository, FiscalDocumentRepository>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }







