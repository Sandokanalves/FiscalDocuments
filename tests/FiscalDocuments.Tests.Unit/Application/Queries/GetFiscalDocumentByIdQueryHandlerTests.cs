using FluentAssertions;
using FiscalDocuments.Application.Features.FiscalDocuments.Queries.GetById;
using FiscalDocuments.Domain.Entities;
using FiscalDocuments.Domain.Interfaces;
using Moq;
using Xunit;

namespace FiscalDocuments.Tests.Unit.Application.Queries;

public class GetFiscalDocumentByIdQueryHandlerTests
{
    private readonly Mock<IFiscalDocumentRepository> _repositoryMock;
    private readonly GetFiscalDocumentByIdQueryHandler _handler;

    public GetFiscalDocumentByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IFiscalDocumentRepository>();
        _handler = new GetFiscalDocumentByIdQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFiscalDocumentDto_WhenDocumentExists()
    {
        // Arrange
        var documentId = Guid.NewGuid();
        var fiscalDocument = new FiscalDocument("access-key", "issuer-cnpj", "recipient-cnpj", DateTime.UtcNow, 100);

        _repositoryMock.Setup(r => r.GetByIdAsync(documentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fiscalDocument);

        var query = new GetFiscalDocumentByIdQuery { Id = documentId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessKey.Should().Be(fiscalDocument.AccessKey);
        _repositoryMock.Verify(r => r.GetByIdAsync(documentId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_WhenDocumentDoesNotExist()
    {
        // Arrange
        var documentId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(documentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FiscalDocument?)null);

        var query = new GetFiscalDocumentByIdQuery { Id = documentId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}

