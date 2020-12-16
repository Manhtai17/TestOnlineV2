#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Elearning.G8.Worker.WorkerService/Elearning.G8.Worker.WorkerService.csproj", "Elearning.G8.Worker.WorkerService/"]
COPY ["Elearning.G8.Common.Kafka/Elearning.G8.Common.Kafka.csproj", "Elearning.G8.Common.Kafka/"]
COPY ["Elearning.G8.Exam.Infracstructure/Elearning.G8.Exam.Infracstructure.csproj", "Elearning.G8.Exam.Infracstructure/"]
COPY ["Elearning.G8.Exam.ApplicationCore/Elearning.G8.Exam.ApplicationCore.csproj", "Elearning.G8.Exam.ApplicationCore/"]
RUN dotnet restore "Elearning.G8.Worker.WorkerService/Elearning.G8.Worker.WorkerService.csproj"
COPY . .
WORKDIR "/src/Elearning.G8.Worker.WorkerService"

RUN dotnet build "Elearning.G8.Worker.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Elearning.G8.Worker.WorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENV TZ=Asia/Ho_Chi_Minh
#RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN unlink /etc/localtime && ln -s /usr/share/zoneinfo/Asia/Ho_Chi_Minh /etc/localtime
COPY ./Elearning.G8.Worker.WorkerService/timezone /etc/timezone
ENTRYPOINT ["dotnet", "Elearning.G8.Worker.WorkerService.dll"]
