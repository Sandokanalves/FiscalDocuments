using FiscalDocuments.Domain.Interfaces;
using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Delete;

public class DeleteFiscalDocumentCommandHandler : IRequestHandler<DeleteFiscalDocumentCommand, bool>
{
    private readonly IFiscalDocumentRepository _repository;

    public DeleteFiscalDocumentCommandHandler(IFiscalDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteFiscalDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (document is null)
        {
            return false;
        }

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}

