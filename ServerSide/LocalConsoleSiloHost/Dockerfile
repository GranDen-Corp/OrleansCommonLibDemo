FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ServerSide/LocalConsoleSiloHost/LocalConsoleSiloHost.csproj", "ServerSide/LocalConsoleSiloHost/"]
RUN dotnet restore "ServerSide/LocalConsoleSiloHost/LocalConsoleSiloHost.csproj"
COPY . .
WORKDIR "/src/ServerSide/LocalConsoleSiloHost"
RUN dotnet build "LocalConsoleSiloHost.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "LocalConsoleSiloHost.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LocalConsoleSiloHost.dll"]
