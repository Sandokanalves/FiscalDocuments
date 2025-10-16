using System.Globalization;
using System.Xml.Linq;
using FiscalDocuments.Domain.Entities;
using FiscalDocuments.Domain.Interfaces;
using FiscalDocuments.Domain.ValueObjects;
using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;

public class UploadFiscalDocumentCommandHandler : IRequestHandler<UploadFiscalDocumentCommand, Guid>
{
    private readonly IFiscalDocumentRepository _repository;

    public UploadFiscalDocumentCommandHandler(IFiscalDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(UploadFiscalDocumentCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
        {
            throw new ArgumentException("Arquivo inválido.", nameof(request.File));
        }

        using var reader = new StreamReader(request.File.OpenReadStream());
        var xmlContent = await reader.ReadToEndAsync(cancellationToken);

        var xmlDoc = XDocument.Parse(xmlContent);
        var ns = xmlDoc.Root?.GetDefaultNamespace() ?? XNamespace.None;

        var infNFe = xmlDoc.Descendants(ns + "infNFe").FirstOrDefault();
        if (infNFe is null) throw new InvalidDataException("Tag <infNFe> não encontrada.");

        var accessKey = infNFe.Attribute("Id")?.Value.Replace("NFe", "") ?? Guid.NewGuid().ToString();

        var existingDocument = await _repository.GetByAccessKeyAsync(accessKey, cancellationToken);
        if (existingDocument != null)
        {
            return existingDocument.Id;
        }

        var ide = infNFe.Element(ns + "ide");
        var emit = infNFe.Element(ns + "emit");
        var dest = infNFe.Element(ns + "dest");
        var total = infNFe.Element(ns + "total")?.Element(ns + "ICMSTot");

        var issuerAddress = new Address(
            emit?.Element(ns + "enderEmit")?.Element(ns + "xLgr")?.Value ?? "",
            emit?.Element(ns + "enderEmit")?.Element(ns + "nro")?.Value ?? "",
            emit?.Element(ns + "enderEmit")?.Element(ns + "xBairro")?.Value ?? "",
            emit?.Element(ns + "enderEmit")?.Element(ns + "cMun")?.Value ?? "",
            emit?.Element(ns + "enderEmit")?.Element(ns + "xMun")?.Value ?? "",
            emit?.Element(ns + "enderEmit")?.Element(ns + "UF")?.Value ?? "",
            emit?.Element(ns + "enderEmit")?.Element(ns + "CEP")?.Value ?? ""
        );

        var recipientAddress = new Address(
            dest?.Element(ns + "enderDest")?.Element(ns + "xLgr")?.Value ?? "",
            dest?.Element(ns + "enderDest")?.Element(ns + "nro")?.Value ?? "",
            dest?.Element(ns + "enderDest")?.Element(ns + "xBairro")?.Value ?? "",
            dest?.Element(ns + "enderDest")?.Element(ns + "cMun")?.Value ?? "",
            dest?.Element(ns + "enderDest")?.Element(ns + "xMun")?.Value ?? "",
            dest?.Element(ns + "enderDest")?.Element(ns + "UF")?.Value ?? "",
            dest?.Element(ns + "enderDest")?.Element(ns + "CEP")?.Value ?? ""
        );

        var fiscalDocument = new FiscalDocument(
            accessKey: accessKey,
            model: int.Parse(ide?.Element(ns + "mod")?.Value ?? "0"),
            series: int.Parse(ide?.Element(ns + "serie")?.Value ?? "0"),
            documentNumber: int.Parse(ide?.Element(ns + "nNF")?.Value ?? "0"),
            issueDate: DateTime.Parse(ide?.Element(ns + "dhEmi")?.Value ?? DateTime.UtcNow.ToString()),
            issuerName: emit?.Element(ns + "xNome")?.Value ?? "",
            issuerCnpj: emit?.Element(ns + "CNPJ")?.Value ?? "",
            issuerAddress: issuerAddress,
            recipientName: dest?.Element(ns + "xNome")?.Value ?? "",
            recipientDocument: dest?.Element(ns + "CNPJ")?.Value ?? dest?.Element(ns + "CPF")?.Value ?? "",
            recipientAddress: recipientAddress,
            totalProducts: decimal.Parse(total?.Element(ns + "vProd")?.Value ?? "0", CultureInfo.InvariantCulture),
            totalAmount: decimal.Parse(total?.Element(ns + "vNF")?.Value ?? "0", CultureInfo.InvariantCulture)
        );

        foreach (var det in infNFe.Elements(ns + "det"))
        {
            var prod = det.Element(ns + "prod");
            var item = new ProductItem(
                productCode: prod?.Element(ns + "cProd")?.Value ?? "",
                description: prod?.Element(ns + "xProd")?.Value ?? "",
                ncm: prod?.Element(ns + "NCM")?.Value ?? "",
                cfop: int.Parse(prod?.Element(ns + "CFOP")?.Value ?? "0"),
                quantity: decimal.Parse(prod?.Element(ns + "qCom")?.Value ?? "0", CultureInfo.InvariantCulture),
                unitPrice: decimal.Parse(prod?.Element(ns + "vUnCom")?.Value ?? "0", CultureInfo.InvariantCulture),
                totalPrice: decimal.Parse(prod?.Element(ns + "vProd")?.Value ?? "0", CultureInfo.InvariantCulture)
            );
            fiscalDocument.AddItem(item);
        }

        await _repository.AddAsync(fiscalDocument, cancellationToken);

        return fiscalDocument.Id;
    }
}

