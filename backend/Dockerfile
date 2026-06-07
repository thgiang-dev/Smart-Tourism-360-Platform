# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["SmartTourism360.Api/SmartTourism360.Api.csproj", "SmartTourism360.Api/"]
COPY ["SmartTourism360.Application/SmartTourism360.Application.csproj", "SmartTourism360.Application/"]
COPY ["SmartTourism360.Domain/SmartTourism360.Domain.csproj", "SmartTourism360.Domain/"]
COPY ["SmartTourism360.Infrastructure/SmartTourism360.Infrastructure.csproj", "SmartTourism360.Infrastructure/"]

RUN dotnet restore "SmartTourism360.Api/SmartTourism360.Api.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish in Release mode
WORKDIR "/src/SmartTourism360.Api"
RUN dotnet publish "SmartTourism360.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmartTourism360.Api.dll"]
