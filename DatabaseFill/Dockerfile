# FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine-amd64 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-amd64 AS build
# WORKDIR /src
# COPY ["DatabaseFill/DatabaseFill.csproj", "DatabaseFill/"]
# RUN dotnet restore "DatabaseFill/DatabaseFill.csproj"
# COPY . .
# WORKDIR "/src/DatabaseFill"
# RUN dotnet build "DatabaseFill.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "DatabaseFill.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "DatabaseFill.dll"]

# Can't run docker build 'cause in image there is no MovieAppAPI dir
# and I cannot move out of build context to copy it
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-amd64 AS build
ENV ENVIRONMENT = "stage"
WORKDIR /src
COPY ["DatabaseFill.csproj", "DatabaseFill/"]
RUN dotnet restore "DatabaseFill/DatabaseFill.csproj"
WORKDIR "/src/DatabaseFill"
COPY . .
RUN dotnet build "DatabaseFill.csproj" -c Release -o /app/build \
    && dotnet publish "DatabaseFill.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine-amd64 AS base
RUN addgroup -g 2000 vladovello \
    && adduser -u 2000 vladovello -G vladovello -s /bin/sh -D vladovello
USER vladovello
LABEL maintainer = "Vlad Suhankov"
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DatabaseFill.dll"]