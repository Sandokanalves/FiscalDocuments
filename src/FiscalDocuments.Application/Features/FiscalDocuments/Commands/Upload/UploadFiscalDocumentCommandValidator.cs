using FluentValidation;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;

public class UploadFiscalDocumentCommandValidator : AbstractValidator<UploadFiscalDocumentCommand>
{
    public UploadFiscalDocumentCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("O arquivo não pode ser nulo.")
            .Must(x => x.Length > 0).WithMessage("O arquivo não pode estar vazio.");
    }
}


