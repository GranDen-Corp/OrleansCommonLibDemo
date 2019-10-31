FROM alpine:3.10 AS base
WORKDIR /app_grain

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /src
COPY ["ServerSide/Grains/NumberGenerator.Grain/NumberGenerator.Grain.csproj", "ServerSide/Grains/NumberGenerator.Grain/"]
COPY ["RPC_Share/NumberGenerator.ShareInterface/NumberGenerator.ShareInterface.csproj", "RPC_Share/NumberGenerator.ShareInterface/"]
RUN dotnet restore "ServerSide/Grains/NumberGenerator.Grain/NumberGenerator.Grain.csproj"
COPY . .
WORKDIR /src/ServerSide/Grains/NumberGenerator.Grain
RUN dotnet build "NumberGenerator.Grain.csproj" -c Release -o /app_grain

FROM build as publish
RUN dotnet publish "NumberGenerator.Grain.csproj" -c Release -o /app_grain

FROM base AS final
WORKDIR /app_grain
COPY --from=publish /app_grain .
VOLUME ["/app_grain"]
CMD ["/bin/true"]
