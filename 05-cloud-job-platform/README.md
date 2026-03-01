# 🚀 Cloud Job Processing Platform

## Tech Stack

- ASP.NET Core
- Clean Architecture
- CQRS + MediatR
- Amazon S3 (LocalStack)
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

## Run Locally

### 1. Start LocalStack

docker-compose up -d

### 2. Run Backend

dotnet run

### 3. Run Frontend

npm install
ng serve
