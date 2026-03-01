using CloudJobEngine.Application.Interfaces;
using MediatR;

namespace CloudJobEngine.Application.Commands.StartJobProcessing;

public class StartJobProcessingHandler
    : IRequestHandler<StartJobProcessingCommand, Unit>
{
    private readonly IJobRepository _jobRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartJobProcessingHandler(
        IJobRepository jobRepository,
        IUnitOfWork unitOfWork)
    {
        _jobRepository = jobRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        StartJobProcessingCommand command,
        CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(command.JobId, cancellationToken)
                  ?? throw new Exception("Job not found");

        job.StartProcessing();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}