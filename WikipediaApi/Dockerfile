FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY WikipediaApi.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish WikipediaApi.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 5040
ENTRYPOINT ["dotnet", "WikipediaApi.dll"]
