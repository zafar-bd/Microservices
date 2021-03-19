#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/ui/Ui.Blazor.WA/Ui.Blazor.WA.csproj", "src/ui/Ui.Blazor.WA/"]
RUN dotnet restore "src/ui/Ui.Blazor.WA/Ui.Blazor.WA.csproj"
COPY . .
WORKDIR "/src/src/ui/Ui.Blazor.WA"
RUN dotnet build "Ui.Blazor.WA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ui.Blazor.WA.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ui.Blazor.WA.dll"]