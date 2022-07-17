#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Backend/WebApi/MarvalWebApi.csproj", "Backend/WebApi/"]
COPY ["Backend/Application/Application.csproj", "Backend/Application/"]
COPY ["Backend/Common.Core/Common.Core.csproj", "Backend/Common.Core/"]
COPY ["Backend/Common/Common.csproj", "Backend/Common/"]
COPY ["Backend/Infrastructure/Infrastructure.csproj", "Backend/Infrastructure/"]
RUN dotnet restore "Backend/WebApi/MarvalWebApi.csproj"
COPY . .
WORKDIR "/src/Backend/WebApi"
RUN dotnet build "MarvalWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarvalWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarvalMarvalWebApi.dll"]