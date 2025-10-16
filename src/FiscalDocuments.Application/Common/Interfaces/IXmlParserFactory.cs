using System.Xml.Linq;

namespace FiscalDocuments.Application.Common.Interfaces;

public interface IXmlParserFactory
{
    IXmlParserStrategy GetParser(XDocument xmlDoc);
}

