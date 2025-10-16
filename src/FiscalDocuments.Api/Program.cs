using FiscalDocuments.Application.Common.Interfaces;
using FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;
using FiscalDocuments.Application.Services.XmlParser;
using FiscalDocuments.Domain.Interfaces;
using FiscalDocuments.Infrastructure.Persistence.Context;
using FiscalDocuments.Infrastructure.Persistence.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(UploadFiscalDocumentCommand).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(UploadFiscalDocumentCommand).Assembly);

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddScoped<IFiscalDocumentRepository, FiscalDocumentRepository>();

builder.Services.AddScoped<IXmlParserStrategy, NfeParserStrategy>();
builder.Services.AddScoped<IXmlParserStrategy, CteParserStrategy>();
builder.Services.AddScoped<IXmlParserStrategy, NfseParserStrategy>();
builder.Services.AddScoped<IXmlParserFactory, XmlParserFactory>();

builder.Services.AddHttpContextAccessor();
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

app.MapControllers();

app.Run();

public partial class Program { }


