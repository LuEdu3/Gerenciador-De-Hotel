## Build and run ASP.NET Core 8 app for Railway
ARG BUILD_CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas os manifestos primeiro para cachear o restore
COPY GerenciadorHotel.csproj ./
COPY Gerenciador-De-Hotel.sln ./
RUN dotnet restore "GerenciadorHotel.csproj"

# Copia todo o restante do código
COPY . .

# Publica a aplicação
RUN dotnet publish "GerenciadorHotel.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "GerenciadorHotel.dll"]
