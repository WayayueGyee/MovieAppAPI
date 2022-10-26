FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MovieAppAPI/MovieAppAPI.csproj", "MovieAppAPI/"]
RUN dotnet restore "MovieAppAPI/MovieAppAPI.csproj"
COPY . .
WORKDIR "/src/MovieAppAPI"
RUN dotnet build "MovieAppAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MovieAppAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieAppAPI.dll"]
