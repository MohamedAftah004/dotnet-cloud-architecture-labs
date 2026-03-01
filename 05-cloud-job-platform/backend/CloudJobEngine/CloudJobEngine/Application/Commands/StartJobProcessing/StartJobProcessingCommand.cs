using MediatR;

namespace CloudJobEngine.Application.Commands.StartJobProcessing;

public record StartJobProcessingCommand(Guid JobId) : IRequest<Unit>;