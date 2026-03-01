using CloudJobEngine.Application.Interfaces;
using MediatR;

namespace CloudJobEngine.Application.Commands.FailJob;

public class FailJobHandler : IRequestHandler<FailJobCommand, Unit>
{
    private readonly IJobRepository _jobRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FailJobHandler(
        IJobRepository jobRepository,
        IUnitOfWork unitOfWork)
    {
        _jobRepository = jobRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        FailJobCommand command,
        CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(command.JobId, cancellationToken)
                  ?? throw new Exception("Job not found");

        job.Fail();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}