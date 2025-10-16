using System.Xml.Linq;
using FiscalDocuments.Domain.Entities;

namespace FiscalDocuments.Application.Common.Interfaces;

public interface IXmlParserStrategy
{
    bool CanParse(XDocument xmlDoc);
    FiscalDocument Parse(XDocument xmlDoc);
}

