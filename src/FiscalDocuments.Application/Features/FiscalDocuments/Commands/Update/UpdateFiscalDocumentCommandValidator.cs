using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Update;

public class UpdateFiscalDocumentCommandValidator : AbstractValidator<UpdateFiscalDocumentCommand>
{
    public UpdateFiscalDocumentCommandValidator(IHttpContextAccessor httpContextAccessor)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O Id do documento é obrigatório.");

        When(x => httpContextAccessor.HttpContext is not null, () =>
        {
            RuleFor(x => x.Id)
                .Must(id => id.ToString() == httpContextAccessor.HttpContext!.Request.RouteValues["id"]!.ToString())
                .WithMessage("O ID do corpo da requisição deve ser igual ao ID da rota.");
        });
    }
}

