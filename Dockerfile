FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5210

ENV ASPNETCORE_URLS=http://+:5210

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN useradd -ms /bin/bash appuser
RUN chmod -R 777 /app
USER root

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Producer.csproj", "./"]
RUN dotnet new tool-manifest
RUN dotnet tool install --local dotnet-ef
RUN dotnet restore "Producer.csproj"



COPY . .
WORKDIR "/src/."
RUN dotnet build "Producer.csproj" -c Release -o /app/build
RUN dotnet ef migrations add MigrationDocker
RUN dotnet ef database update --connection "Server=localhost;Database=Producer1;User=sa;password=Shiv@angi1Sing;Persist Security info=True;TrustServerCertificate=True;"
FROM build AS publish
RUN dotnet publish "Producer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER appuser
ENTRYPOINT [ "dotnet","Producer.dll" ]
