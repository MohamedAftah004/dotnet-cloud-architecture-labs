using MediatR;

namespace CloudJobEngine.Application.Commands.CompleteJob;

public record CompleteJobCommand(Guid JobId) : IRequest<Unit>;