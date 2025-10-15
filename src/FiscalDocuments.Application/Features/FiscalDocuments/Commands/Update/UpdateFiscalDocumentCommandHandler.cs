using FiscalDocuments.Domain.Interfaces;
using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Update;

public class UpdateFiscalDocumentCommandHandler : IRequestHandler<UpdateFiscalDocumentCommand, bool>
{
    private readonly IFiscalDocumentRepository _repository;

    public UpdateFiscalDocumentCommandHandler(IFiscalDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateFiscalDocumentCommand request, CancellationToken cancellationToken)
    {
        var fiscalDocument = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (fiscalDocument is null)
        {
            return false;
        }

        fiscalDocument.UpdateTotalAmount(request.TotalAmount);

        await _repository.UpdateAsync(fiscalDocument, cancellationToken);

        return true;
    }
}

