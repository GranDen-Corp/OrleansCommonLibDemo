FROM alpine:3.8 AS base
WORKDIR /app_grain

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ServerSide/Grains/MyReminder.Grain/MyReminder.Grain.csproj", "ServerSide/Grains/MyReminder.Grain/"]
COPY ["RPC_Share/MyReminder.ShareInterface/MyReminder.ShareInterface.csproj", "RPC_Share/MyReminder.ShareInterface/"]
RUN dotnet restore "ServerSide/Grains/MyReminder.Grain/MyReminder.Grain.csproj"
COPY . .
WORKDIR /src/ServerSide/Grains/MyReminder.Grain
RUN dotnet build "MyReminder.Grain.csproj" -c Release -o /app_grain

FROM build as publish
RUN dotnet publish "MyReminder.Grain.csproj" -c Release -o /app_grain

FROM base AS final
WORKDIR /app_grain
COPY --from=publish /app_grain .
VOLUME ["/app_grain"]
CMD ["/bin/true"]
