#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/workers/Product.Sales.Worker/Product.Sales.Worker.csproj", "src/workers/Product.Sales.Worker/"]
RUN dotnet restore "src/workers/Product.Sales.Worker/Product.Sales.Worker.csproj"
COPY . .
WORKDIR "/src/src/workers/Product.Sales.Worker"
RUN dotnet build "Product.Sales.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Product.Sales.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.Sales.Worker.dll"]