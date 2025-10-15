using System.Reflection;
using FluentAssertions;
using FiscalDocuments.Domain.Entities;
using FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;
using FiscalDocuments.Infrastructure.Persistence.Context;
using FiscalDocuments.Api.Controllers;
using NetArchTest.Rules;
using Xunit;

namespace FiscalDocuments.Tests.Architecture;

public class ArchitectureTests
{
    private static readonly Assembly DomainAssembly = typeof(FiscalDocument).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(UploadFiscalDocumentCommand).Assembly;
    private static readonly Assembly InfrastructureAssembly = typeof(AppDbContext).Assembly;
    private static readonly Assembly ApiAssembly = typeof(FiscalDocumentsController).Assembly;

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var otherProjects = new[]
        {
            ApplicationAssembly.GetName().Name,
            InfrastructureAssembly.GetName().Name,
            ApiAssembly.GetName().Name
        };

        // Act
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}

