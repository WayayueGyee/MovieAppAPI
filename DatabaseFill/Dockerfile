FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-amd64 AS build
WORKDIR /src
COPY ["DatabaseFill/DatabaseFill.csproj", "DatabaseFill/"]
RUN dotnet restore "DatabaseFill/DatabaseFill.csproj"
COPY . .
WORKDIR "/src/DatabaseFill"
RUN dotnet build "DatabaseFill.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DatabaseFill.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DatabaseFill.dll"]
