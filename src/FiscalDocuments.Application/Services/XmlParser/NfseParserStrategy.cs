using System.Globalization;
using System.Xml.Linq;
using FiscalDocuments.Application.Common.Interfaces;
using FiscalDocuments.Domain.Entities;
using FiscalDocuments.Domain.ValueObjects;

namespace FiscalDocuments.Application.Services.XmlParser;

public class NfseParserStrategy : IXmlParserStrategy
{
    public bool CanParse(XDocument xmlDoc)
    {
        return xmlDoc.Root?.Name.LocalName == "EnviarLoteRpsEnvio" || xmlDoc.Descendants().Any(e => e.Name.LocalName == "InfDeclaracaoPrestacaoServico");
    }

    public FiscalDocument Parse(XDocument xmlDoc)
    {
        var ns = xmlDoc.Root?.GetDefaultNamespace() ?? XNamespace.None;
        var infServico = xmlDoc.Descendants(ns + "InfDeclaracaoPrestacaoServico").FirstOrDefault() ?? throw new InvalidDataException("Tag <InfDeclaracaoPrestacaoServico> não encontrada.");

        var prestador = infServico.Element(ns + "Prestador");
        var rps = infServico.Element(ns + "Rps")?.Element(ns + "IdentificacaoRps");
        var accessKey = $"{prestador?.Element(ns + "Cnpj")?.Value}-{rps?.Element(ns + "Numero")?.Value}";

        var tomador = infServico.Element(ns + "Tomador");
        var valores = infServico.Element(ns + "Servico")?.Element(ns + "Valores");

        var issuerAddress = new Address("", "", "", "", "", "", "");

        var recipientAddress = new Address(
            tomador?.Element(ns + "Endereco")?.Element(ns + "Endereco")?.Value ?? "",
            tomador?.Element(ns + "Endereco")?.Element(ns + "Numero")?.Value ?? "",
            tomador?.Element(ns + "Endereco")?.Element(ns + "Bairro")?.Value ?? "",
            tomador?.Element(ns + "Endereco")?.Element(ns + "CodigoMunicipio")?.Value ?? "",
            "",
            tomador?.Element(ns + "Endereco")?.Element(ns + "Uf")?.Value ?? "",
            tomador?.Element(ns + "Endereco")?.Element(ns + "Cep")?.Value ?? ""
        );

        var fiscalDocument = new FiscalDocument(
            accessKey: accessKey,
            model: 99,
            series: rps?.Element(ns + "Serie")?.Value ?? "",
            documentNumber: int.Parse(rps?.Element(ns + "Numero")?.Value ?? "0"),
            issueDate: DateTime.Parse(infServico.Element(ns + "Rps")?.Element(ns + "DataEmissao")?.Value ?? DateTime.UtcNow.ToString()),
            issuerName: "",
            issuerCnpj: prestador?.Element(ns + "Cnpj")?.Value ?? "",
            issuerAddress: issuerAddress,
            recipientName: tomador?.Element(ns + "RazaoSocial")?.Value ?? "",
            recipientDocument: tomador?.Element(ns + "IdentificacaoTomador")?.Element(ns + "CpfCnpj")?.Element(ns + "Cnpj")?.Value ?? tomador?.Element(ns + "IdentificacaoTomador")?.Element(ns + "CpfCnpj")?.Element(ns + "Cpf")?.Value ?? "",
            recipientAddress: recipientAddress,
            totalProducts: 0,
            totalAmount: decimal.Parse(valores?.Element(ns + "ValorServicos")?.Value ?? "0", CultureInfo.InvariantCulture)
        );

        return fiscalDocument;
    }
}

