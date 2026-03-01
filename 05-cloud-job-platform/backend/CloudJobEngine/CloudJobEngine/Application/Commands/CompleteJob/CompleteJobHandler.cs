using CloudJobEngine.Application.Interfaces;
using MediatR;

namespace CloudJobEngine.Application.Commands.CompleteJob;

public class CompleteJobHandler : IRequestHandler<CompleteJobCommand, Unit>
{
    private readonly IJobRepository _jobRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteJobHandler(
        IJobRepository jobRepository,
        IUnitOfWork unitOfWork)
    {
        _jobRepository = jobRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        CompleteJobCommand command,
        CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(command.JobId, cancellationToken)
                  ?? throw new Exception("Job not found");

        job.Complete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}