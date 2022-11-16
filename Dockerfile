# specify build image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /build
COPY . .

# create the build
RUN dotnet restore
# RUN dotnet new tool-manifest
RUN dotnet tool install dotnet-ef --version 3.1.0
RUN dotnet publish -o /app

# run the build
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ScoreStore.dll"]