using FiscalDocuments.Application.Features.FiscalDocuments.Commands.Delete;
using FiscalDocuments.Application.Features.FiscalDocuments.Commands.Update;
using FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;
using FiscalDocuments.Application.Features.FiscalDocuments.Queries.GetById;
using FiscalDocuments.Application.Features.FiscalDocuments.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FiscalDocuments.Api.Controllers;

public class FiscalDocumentsController : MainController
{
    private readonly IMediator _mediator;

    public FiscalDocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadDocument(IFormFile file)
    {
        var command = new UploadFiscalDocumentCommand { File = file };
        var documentId = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetDocumentById), new { id = documentId }, new { id = documentId });
    }

    [HttpGet]
    public async Task<IActionResult> ListDocuments([FromQuery] ListFiscalDocumentsQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [HttpGet("{id:guid}", Name = "GetDocumentById")]
    public async Task<IActionResult> GetDocumentById(Guid id)
    {
        return HandleResult(await _mediator.Send(new GetFiscalDocumentByIdQuery { Id = id }));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateDocument(Guid id, [FromBody] UpdateFiscalDocumentCommand command)
    {
        // A validação que compara 'id' da rota com 'command.Id' agora é feita
        // automaticamente pelo UpdateFiscalDocumentCommandValidator.
        return await _mediator.Send(command) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDocument(Guid id)
    {
        return await _mediator.Send(new DeleteFiscalDocumentCommand { Id = id }) ? NoContent() : NotFound();
    }
}


