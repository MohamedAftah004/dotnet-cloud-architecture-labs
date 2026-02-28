using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using AsyncUserProcessing.Application.DTOs;
using AsyncUserProcessing.Domain.Interfaces;
using AsyncUserProcessing.Infrastructure.Messaging;
using AsyncUserProcessing.Infrastructure.Persistence;
using AsyncUserProcessing.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.Configure<AwsOptions>(
    builder.Configuration.GetSection("AWS"));

builder.Services.Configure<AwsOptions>(
    builder.Configuration.GetSection("AWS"));

builder.Services.AddSingleton<IAmazonSQS>(sp =>
{
    var options = sp.GetRequiredService<IOptions<AwsOptions>>().Value;

    var config = new AmazonSQSConfig
    {
        ServiceURL = options.ServiceUrl,
        UseHttp = true
    };

    return new AmazonSQSClient(
        new BasicAWSCredentials(options.AccessKey, options.SecretKey),
        config);
});

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var options = sp.GetRequiredService<IOptions<AwsOptions>>().Value;

    var config = new AmazonS3Config
    {
        ServiceURL = options.ServiceUrl,
        ForcePathStyle = true
    };

    return new AmazonS3Client(
        new BasicAWSCredentials(options.AccessKey, options.SecretKey),
        config);
});

builder.Services.AddSingleton<IImageStorage, S3ImageStorage>(); 
builder.Services.AddSingleton<IEventPublisher, SqsEventPublisher>();
builder.Services.AddSingleton<SqsConsumer>();

builder.Services.AddScoped<UserAppService>();
builder.Services.AddHostedService<UserProcessingWorker>();
builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection("Email"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
