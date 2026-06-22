FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Golinks.WebAPI/Golinks.WebAPI.csproj", "Golinks.WebAPI/"]
COPY ["Golinks.Application/Golinks.Application.csproj", "Golinks.Application/"]
COPY ["Golinks.Domain/Golinks.Domain.csproj", "Golinks.Domain/"]
COPY ["Golinks.Repository/Golinks.Repository.csproj", "Golinks.Repository/"]
RUN dotnet restore "Golinks.WebAPI/Golinks.WebAPI.csproj"
COPY . .
WORKDIR "/src/Golinks.WebAPI"
RUN dotnet build "Golinks.WebAPI.csproj" -c Release --no-restore -o /app/build

FROM build AS publish
RUN dotnet publish "Golinks.WebAPI.csproj" -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Golinks.WebAPI.dll"]
