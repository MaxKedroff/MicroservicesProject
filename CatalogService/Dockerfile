# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CatalogService.csproj", "."]
RUN dotnet restore "CatalogService.csproj"
COPY . .
RUN dotnet build "CatalogService.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CatalogService.csproj" -c Release -o /app/publish



# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "CatalogService.dll"]