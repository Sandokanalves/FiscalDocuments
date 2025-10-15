using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FiscalDocuments.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FiscalDocumentsController : ControllerBase
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
        if (file == null || file.Length == 0)
        {
            return BadRequest("Arquivo inválido.");
        }

        using var reader = new StreamReader(file.OpenReadStream());
        var xmlContent = await reader.ReadToEndAsync();

        var command = new Application.Features.FiscalDocuments.Commands.Upload.UploadFiscalDocumentCommand { XmlContent = xmlContent };
        var documentId = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetDocumentById), new { id = documentId }, new { id = documentId });
    }

    [HttpGet]
    public async Task<IActionResult> ListDocuments()
    {
        var query = new Application.Features.FiscalDocuments.Queries.List.ListFiscalDocumentsQuery();
        var documents = await _mediator.Send(query);
        return Ok(documents);
    }

    [HttpGet("{id:guid}", Name = "GetDocumentById")]
    public async Task<IActionResult> GetDocumentById(Guid id)
    {
        var query = new Application.Features.FiscalDocuments.Queries.GetById.GetFiscalDocumentByIdQuery { Id = id };
        var document = await _mediator.Send(query);
        return document is not null ? Ok(document) : NotFound();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateDocument(Guid id, [FromBody] Application.Features.FiscalDocuments.Commands.Update.UpdateFiscalDocumentCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("O ID da rota não corresponde ao ID do corpo da requisição.");
        }

        var result = await _mediator.Send(command);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDocument(Guid id)
    {
        var command = new Application.Features.FiscalDocuments.Commands.Delete.DeleteFiscalDocumentCommand { Id = id };
        var result = await _mediator.Send(command);
        return result ? NoContent() : NotFound();
    }
}



