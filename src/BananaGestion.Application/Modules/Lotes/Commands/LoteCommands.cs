using BananaGestion.Application.Modules.Lotes.DTOs;
using BananaGestion.Domain.Entities;
using FluentValidation;
using MediatR;

namespace BananaGestion.Application.Modules.Lotes.Commands;

public record CreateLoteCommand(CreateLoteRequest Request) : IRequest<LoteDto>;

public record UpdateLoteCommand(Guid Id, UpdateLoteRequest Request) : IRequest<LoteDto>;

public record DeleteLoteCommand(Guid Id) : IRequest<bool>;

public class CreateLoteValidator : AbstractValidator<CreateLoteCommand>
{
    public CreateLoteValidator()
    {
        RuleFor(x => x.Request.Codigo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Request.Nombre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Request.Hectareas).GreaterThan(0);
    }
}

public class UpdateLoteValidator : AbstractValidator<UpdateLoteCommand>
{
    public UpdateLoteValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
