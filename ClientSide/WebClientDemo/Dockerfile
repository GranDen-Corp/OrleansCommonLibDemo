FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
WORKDIR /src
COPY ["ClientSide/WebClientDemo/WebClientDemo.csproj", "ClientSide/WebClientDemo/"]
COPY ["RPC_Share/MyReminder.ShareInterface/MyReminder.ShareInterface.csproj", "RPC_Share/MyReminder.ShareInterface/"]
COPY ["RPC_Share/HelloWorld.ShareInterface/HelloWorld.ShareInterface.csproj", "RPC_Share/HelloWorld.ShareInterface/"]
COPY ["RPC_Share/NumberGenerator.ShareInterface/NumberGenerator.ShareInterface.csproj", "RPC_Share/NumberGenerator.ShareInterface/"]
RUN dotnet restore "ClientSide/WebClientDemo/WebClientDemo.csproj"
COPY . .
WORKDIR "/src/ClientSide/WebClientDemo"
RUN dotnet build "WebClientDemo.csproj" --framework netcoreapp2.2 -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebClientDemo.csproj" --framework netcoreapp2.2 -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebClientDemo.dll"]
