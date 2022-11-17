FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-amd64 AS build
ENV ENVIRONMENT = "stage"
WORKDIR /src
COPY ["MovieAppAPI.csproj", "MovieAppAPI/"]
RUN dotnet restore "MovieAppAPI/MovieAppAPI.csproj"
WORKDIR "/src/MovieAppAPI"
COPY . .
RUN dotnet build "MovieAppAPI.csproj" -c Release -o /app/build \
    && dotnet publish "MovieAppAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine-amd64 AS base
RUN addgroup -g 2000 vladovello \
    && adduser -u 2000 vladovello -G vladovello -s /bin/sh -D vladovello
USER vladovello
LABEL maintainer = "Vlad Suhankov"
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MovieAppAPI.dll"]