using Amazon.SQS;
using MiniOrderApi.BackgroundWorkers;
using MiniOrderApi.Messaging;
using MiniOrderApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//assign aws sqs service
builder.Services.AddSingleton<IAmazonSQS>(sp =>
{
    var config = new AmazonSQSConfig
    {
        ServiceURL = "http://localhost:4566"
    };

    return new AmazonSQSClient(config);
});

builder.Services.AddSingleton<SqsPublisher>();
builder.Services.AddSingleton<SqsConsumer>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddHostedService<OrderPaymentWorker>();



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
