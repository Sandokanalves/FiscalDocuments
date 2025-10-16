using System.Xml.Linq;
using FiscalDocuments.Application.Common.Interfaces;

namespace FiscalDocuments.Application.Services.XmlParser;

public class XmlParserFactory : IXmlParserFactory
{
    private readonly IEnumerable<IXmlParserStrategy> _strategies;

    public XmlParserFactory(IEnumerable<IXmlParserStrategy> strategies)
    {
        _strategies = strategies;
    }

    public IXmlParserStrategy GetParser(XDocument xmlDoc)
    {
        var strategy = _strategies.FirstOrDefault(s => s.CanParse(xmlDoc));
        return strategy ?? throw new NotSupportedException("Tipo de documento XML não suportado.");
    }
}


