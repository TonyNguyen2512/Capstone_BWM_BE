#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WorkshopManagementSystem_BWM/WorkshopManagementSystem_BWM.csproj", "WorkshopManagementSystem/"]
RUN dotnet restore "WorkshopManagementSystem/WorkshopManagementSystem_BWM.csproj"
COPY . .
WORKDIR "/src/WorkshopManagementSystem"
RUN dotnet build "WorkshopManagementSystem_BWM.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkshopManagementSystem_BWM.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkshopManagementSystem.dll"]