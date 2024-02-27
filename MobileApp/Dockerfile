FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y wget

WORKDIR /app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["TicketWebApp/TicketWebApp.csproj", "TicketWebApp/"]
RUN dotnet restore "TicketWebApp/TicketWebApp.csproj"

COPY . .
WORKDIR /src/TicketWebApp
RUN pwd; ls -la
RUN dotnet build "TicketWebApp.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "TicketWebApp.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketWebApp.dll"]
