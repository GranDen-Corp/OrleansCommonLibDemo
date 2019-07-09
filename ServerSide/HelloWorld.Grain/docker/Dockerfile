FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app_grain

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ServerSide/HelloWorld.Grain/HelloWorld.Grain.csproj", "ServerSide/HelloWorld.Grain/"]
COPY ["RPC_Share/VisitTracker.Interface/VisitTracker.Interface.csproj", "RPC_Share/VisitTracker.Interface/"]
COPY ["RPC_Share/HelloWorld.ShareInterface/HelloWorld.ShareInterface.csproj", "RPC_Share/HelloWorld.ShareInterface/"]
RUN dotnet restore "ServerSide/HelloWorld.Grain/HelloWorld.Grain.csproj"
COPY . .
WORKDIR /src/ServerSide/HelloWorld.Grain
RUN dotnet build "HelloWorld.Grain.csproj" -c Release -o /app_grain

FROM build as publish
RUN dotnet publish "HelloWorld.Grain.csproj" -c Release -o /app_grain

FROM base AS final
WORKDIR /app_grain
COPY --from=publish /app_grain .
VOLUME ["/app_grain"]
CMD ["/bin/true"]
