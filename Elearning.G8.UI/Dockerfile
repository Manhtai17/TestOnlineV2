#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Elearning.G8.UI/Elearning.G8.UI.csproj", "Elearning.G8.UI/"]
RUN dotnet restore "Elearning.G8.UI/Elearning.G8.UI.csproj"
COPY . .
WORKDIR "/src/Elearning.G8.UI"
RUN dotnet build "Elearning.G8.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Elearning.G8.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elearning.G8.UI.dll"]