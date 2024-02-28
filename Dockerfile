FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LR6_WEB_NET/LR6_WEB_NET.csproj", "LR6_WEB_NET/"]
RUN dotnet restore "LR6_WEB_NET/LR6_WEB_NET.csproj"
COPY . .
WORKDIR "/src/LR6_WEB_NET"
RUN dotnet build "LR6_WEB_NET.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LR6_WEB_NET.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "LR6_WEB_NET.dll"]
