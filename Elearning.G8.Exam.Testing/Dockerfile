#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Elearning.G8.Exam.Testing/Elearning.G8.Exam.Testing.csproj", "Elearning.G8.Exam.Testing/"]
COPY ["Elearning.G8.Common.Kafka/Elearning.G8.Common.Kafka.csproj", "Elearning.G8.Common.Kafka/"]
COPY ["Elearning.G8.Exam.Infracstructure/Elearning.G8.Exam.Infracstructure.csproj", "Elearning.G8.Exam.Infracstructure/"]
COPY ["Elearning.G8.Exam.ApplicationCore/Elearning.G8.Exam.ApplicationCore.csproj", "Elearning.G8.Exam.ApplicationCore/"]
RUN dotnet restore "Elearning.G8.Exam.Testing/Elearning.G8.Exam.Testing.csproj"
COPY . .
WORKDIR "/src/Elearning.G8.Exam.Testing"
RUN dotnet build "Elearning.G8.Exam.Testing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Elearning.G8.Exam.Testing.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN unlink /etc/localtime && ln -s /usr/share/zoneinfo/Asia/Ho_Chi_Minh /etc/localtime
COPY ./Elearning.G8.Exam.Testing/timezone /etc/timezone
ENTRYPOINT ["dotnet", "Elearning.G8.Exam.Testing.dll"]
