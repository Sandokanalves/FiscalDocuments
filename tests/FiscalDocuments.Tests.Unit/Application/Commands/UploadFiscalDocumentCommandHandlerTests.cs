using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Moq;
using FluentAssertions;
using FiscalDocuments.Domain.Interfaces;
using FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;
using FiscalDocuments.Application.Common.Interfaces;
using FiscalDocuments.Domain.Entities;
using FiscalDocuments.Domain.ValueObjects;

namespace FiscalDocuments.Tests.Unit.Application.Commands;

public class UploadFiscalDocumentCommandHandlerTests
{
    private readonly Mock<IFiscalDocumentRepository> _repositoryMock;
    private readonly Mock<IXmlParserFactory> _factoryMock;
    private readonly Mock<IXmlParserStrategy> _strategyMock;
    private readonly UploadFiscalDocumentCommandHandler _handler;

    public UploadFiscalDocumentCommandHandlerTests()
    {
        _repositoryMock = new Mock<IFiscalDocumentRepository>();
        _factoryMock = new Mock<IXmlParserFactory>();
        _strategyMock = new Mock<IXmlParserStrategy>();

        _handler = new UploadFiscalDocumentCommandHandler(_repositoryMock.Object, _factoryMock.Object);
    }

    private IFormFile CreateTestFile(string content)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var file = new FormFile(stream, 0, stream.Length, "file", "test.xml")
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/xml"
        };
        return file;
    }

    [Fact]
    public async Task Handle_ShouldProcessNewValidXml_AndReturnNewGuid()
    {
        var xmlContent = "<test><infNFe Id=\"NFe123\"/></test>";
        var testFile = CreateTestFile(xmlContent);
        var command = new UploadFiscalDocumentCommand { File = testFile };

        var dummyAddress = new Address("", "", "", "", "", "", "");
        var parsedDocument = new FiscalDocument("123", 55, "1", 1, DateTime.UtcNow, "Issuer", "111", dummyAddress, "Recipient", "222", dummyAddress, 100, 100);

        _factoryMock.Setup(f => f.GetParser(It.IsAny<XDocument>())).Returns(_strategyMock.Object);
        _strategyMock.Setup(s => s.Parse(It.IsAny<XDocument>())).Returns(parsedDocument);
        _repositoryMock.Setup(r => r.GetByAccessKeyAsync("123", It.IsAny<CancellationToken>()))
            .ReturnsAsync((FiscalDocument)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(parsedDocument.Id);
        _repositoryMock.Verify(r => r.AddAsync(It.Is<FiscalDocument>(doc => doc.AccessKey == "123"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnExistingId_WhenDocumentAlreadyExists()
    {
        var xmlContent = "<test><infNFe Id=\"NFe456\"/></test>";
        var testFile = CreateTestFile(xmlContent);
        var command = new UploadFiscalDocumentCommand { File = testFile };

        var dummyAddress = new Address("", "", "", "", "", "", "");
        var existingDocument = new FiscalDocument("456", 55, "1", 2, DateTime.UtcNow, "Issuer", "111", dummyAddress, "Recipient", "222", dummyAddress, 200, 200);

        _factoryMock.Setup(f => f.GetParser(It.IsAny<XDocument>())).Returns(_strategyMock.Object);
        _strategyMock.Setup(s => s.Parse(It.IsAny<XDocument>())).Returns(existingDocument);
        _repositoryMock.Setup(r => r.GetByAccessKeyAsync("456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingDocument);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(existingDocument.Id);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<FiscalDocument>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenXmlIsNotSupported()
    {
        var xmlContent = "<unsupportedschema></unsupportedschema>";
        var testFile = CreateTestFile(xmlContent);
        var command = new UploadFiscalDocumentCommand { File = testFile };

        _factoryMock.Setup(f => f.GetParser(It.IsAny<XDocument>()))
            .Throws(new NotSupportedException("Tipo de documento XML não suportado."));

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotSupportedException>()
            .WithMessage("Tipo de documento XML não suportado.");
    }
}

