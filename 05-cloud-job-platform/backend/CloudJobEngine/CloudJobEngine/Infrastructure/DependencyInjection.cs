using Amazon.S3;
using Amazon.SQS;
using CloudJobEngine.Application.Interfaces;
using CloudJobEngine.Infrastructure.Messaging;
using CloudJobEngine.Infrastructure.Notifications;
using CloudJobEngine.Infrastructure.Persistence;
using CloudJobEngine.Infrastructure.Persistence.Repositories;
using CloudJobEngine.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;

namespace CloudJobEngine.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"))
                );

            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IFileStorageService, S3FileStorageService>();
            services.AddScoped<IMessageQueueService, SqsMessageQueueService>();
            services.AddScoped<INotificationService, FakeNotificationService>();
            services.AddHostedService<SqsWorkerBackgroundService>();

            services.AddDefaultAWSOptions(configuration.GetAWSOptions());

            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonSQS>();
            return services;

        }


    }
}
