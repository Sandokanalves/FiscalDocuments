using FluentValidation;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;

public class UploadFiscalDocumentCommandValidator : AbstractValidator<UploadFiscalDocumentCommand>
{
    public UploadFiscalDocumentCommandValidator()
    {
        RuleFor(x => x.XmlContent)
            .NotEmpty().WithMessage("O conteúdo do XML não pode ser vazio.");
    }
}

