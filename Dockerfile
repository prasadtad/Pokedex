#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Pokedex.WebApi/Pokedex.WebApi.csproj", "Pokedex.WebApi/"]
RUN dotnet restore "Pokedex.WebApi/Pokedex.WebApi.csproj"
COPY . .
WORKDIR "/src/Pokedex.WebApi"
RUN dotnet build "Pokedex.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pokedex.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pokedex.WebApi.dll"]