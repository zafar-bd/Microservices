#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/identity/Identity.IS4/Identity.IS4.csproj", "src/identity/Identity.IS4/"]
RUN dotnet restore "src/identity/Identity.IS4/Identity.IS4.csproj"
COPY . .
WORKDIR "/src/src/identity/Identity.IS4"
RUN dotnet build "Identity.IS4.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.IS4.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.IS4.dll"]
