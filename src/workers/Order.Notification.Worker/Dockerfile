#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/workers/Order.Notification.Worker/Order.Notification.Worker.csproj", "src/workers/Order.Notification.Worker/"]
RUN dotnet restore "src/workers/Order.Notification.Worker/Order.Notification.Worker.csproj"
COPY . .
WORKDIR "/src/src/workers/Order.Notification.Worker"
RUN dotnet build "Order.Notification.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.Notification.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Notification.Worker.dll"]