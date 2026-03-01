using MediatR;

namespace CloudJobEngine.Application.Commands.FailJob;

public record FailJobCommand(Guid JobId) : IRequest<Unit>;