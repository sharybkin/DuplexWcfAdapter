FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /solution/src/app

# Copy csproj and restore as distinct layers
COPY src/Service/BettingLine.Service/BettingLine.Service.csproj ./
COPY src/Service/Common/Common.csproj ./
RUN dotnet restore BettingLine.Service.csproj

# Copy everything else and build
COPY src/Service/BettingLine.Service ./
COPY src/Service/Common ./
RUN dotnet publish BettingLine.Service.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /app
COPY --from=build-env /solution/src/app/out .
ENTRYPOINT ["dotnet", "BettingLine.Service.dll"]