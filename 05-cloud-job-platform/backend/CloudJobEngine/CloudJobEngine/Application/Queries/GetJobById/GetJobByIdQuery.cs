using MediatR;
using CloudJobEngine.Application.DTOs;

namespace CloudJobEngine.Application.Queries.GetJobById;

public record GetJobByIdQuery(Guid JobId) : IRequest<JobDto?>;