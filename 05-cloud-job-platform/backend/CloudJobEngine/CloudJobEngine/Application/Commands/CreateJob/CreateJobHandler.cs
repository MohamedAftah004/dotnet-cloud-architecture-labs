using CloudJobEngine.Application.Interfaces;
using CloudJobEngine.Domain.Entities;
using MediatR;

namespace CloudJobEngine.Application.Commands.CreateJob;

public class CreateJobHandler : IRequestHandler<CreateJobCommand, Guid>
{
    private readonly IJobRepository _jobRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageQueueService _messageQueueService;

    public CreateJobHandler(
        IJobRepository jobRepository,
        IUnitOfWork unitOfWork,
        IMessageQueueService messageQueueService)
    {
        _jobRepository = jobRepository;
        _unitOfWork = unitOfWork;
        _messageQueueService = messageQueueService;
    }

    public async Task<Guid> Handle(
        CreateJobCommand command,
        CancellationToken cancellationToken)
    {
        var job = new Job(command.UserId, command.FileKey);

        await _jobRepository.AddAsync(job, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _messageQueueService.PublishAsync(
            new JobMessage(job.Id, job.FileKey),
            cancellationToken);

        return job.Id;
    }
}