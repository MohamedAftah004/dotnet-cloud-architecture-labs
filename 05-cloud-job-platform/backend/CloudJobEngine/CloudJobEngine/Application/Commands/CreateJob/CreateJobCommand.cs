using MediatR;

namespace CloudJobEngine.Application.Commands.CreateJob;

public record CreateJobCommand(
    Guid UserId,
    string FileKey
) : IRequest<Guid>;