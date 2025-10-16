using System.Globalization;
using System.Xml.Linq;
using FiscalDocuments.Application.Common.Interfaces;
using FiscalDocuments.Domain.Entities;
using FiscalDocuments.Domain.ValueObjects;

namespace FiscalDocuments.Application.Services.XmlParser;

public class CteParserStrategy : IXmlParserStrategy
{
    public bool CanParse(XDocument xmlDoc)
    {
        return xmlDoc.Root?.Name.LocalName == "cteProc" || xmlDoc.Descendants().Any(e => e.Name.LocalName == "infCte");
    }

    public FiscalDocument Parse(XDocument xmlDoc)
    {
        var ns = xmlDoc.Root?.GetDefaultNamespace() ?? XNamespace.None;
        var infCte = xmlDoc.Descendants(ns + "infCte").FirstOrDefault() ?? throw new InvalidDataException("Tag <infCte> não encontrada.");

        var accessKey = infCte.Attribute("Id")?.Value.Replace("CTe", "") ?? Guid.NewGuid().ToString();

        var ide = infCte.Element(ns + "ide");
        var emit = infCte.Element(ns + "emit");
        var dest = infCte.Element(ns + "dest");
        var vPrest = infCte.Element(ns + "vPrest");

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
            series: ide?.Element(ns + "serie")?.Value ?? "0",
            documentNumber: int.Parse(ide?.Element(ns + "nCT")?.Value ?? "0"),
            issueDate: DateTime.Parse(ide?.Element(ns + "dhEmi")?.Value ?? DateTime.UtcNow.ToString()),
            issuerName: emit?.Element(ns + "xNome")?.Value ?? "",
            issuerCnpj: emit?.Element(ns + "CNPJ")?.Value ?? "",
            issuerAddress: issuerAddress,
            recipientName: dest?.Element(ns + "xNome")?.Value ?? "",
            recipientDocument: dest?.Element(ns + "CNPJ")?.Value ?? dest?.Element(ns + "CPF")?.Value ?? "",
            recipientAddress: recipientAddress,
            totalProducts: 0,
            totalAmount: decimal.Parse(vPrest?.Element(ns + "vTPrest")?.Value ?? "0", CultureInfo.InvariantCulture)
        );

        return fiscalDocument;
    }
}

