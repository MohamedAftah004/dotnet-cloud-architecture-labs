# 🚀 Cloud Job Processing Platform

## Tech Stack

- ASP.NET Core
- Clean Architecture
- CQRS + MediatR
- Amazon S3 
- Amazon SQS
- Angular Frontend
- Docker

## Architecture Flow

1. User creates Job
2. Backend generates Presigned URL
3. Front uploads file directly to S3
4. Event published to SQS
5. Worker processes job
6. Status updated

