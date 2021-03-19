#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/workers/Product.Inventory.Worker/Product.Inventory.Worker.csproj", "src/workers/Product.Inventory.Worker/"]
RUN dotnet restore "src/workers/Product.Inventory.Worker/Product.Inventory.Worker.csproj"
COPY . .
WORKDIR "/src/src/workers/Product.Inventory.Worker"
RUN dotnet build "Product.Inventory.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Product.Inventory.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.Inventory.Worker.dll"]