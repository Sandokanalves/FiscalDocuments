using FluentAssertions;
using FiscalDocuments.Application.Features.FiscalDocuments.Queries.GetById;
using FiscalDocuments.Domain.Entities;
using FiscalDocuments.Domain.Interfaces;
using FiscalDocuments.Domain.ValueObjects;
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
        var documentId = Guid.NewGuid();
        var address = new Address("street", "123", "district", "3550308", "Sao Paulo", "SP", "01000000");
        var fiscalDocument = new FiscalDocument(
            "access-key", 55, 1, 123, DateTime.UtcNow,
            "Issuer Name", "11111111000111", address,
            "Recipient Name", "22222222000122", address,
            100, 100
        );

        _repositoryMock.Setup(r => r.GetByIdAsync(documentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fiscalDocument);

        var query = new GetFiscalDocumentByIdQuery { Id = documentId };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.AccessKey.Should().Be(fiscalDocument.AccessKey);
        _repositoryMock.Verify(r => r.GetByIdAsync(documentId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_WhenDocumentDoesNotExist()
    {
        var documentId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(documentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FiscalDocument?)null);

        var query = new GetFiscalDocumentByIdQuery { Id = documentId };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}

