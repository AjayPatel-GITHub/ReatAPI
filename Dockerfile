#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#GET base SDK Image from Microsoft
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
RUN mkdir -p /app/Photos

#Copy the csproj file and restore any dependencies (via NUGET)
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ReactAPI.csproj", "."]
RUN dotnet restore "./ReactAPI.csproj"
# Copy the project file and build our release
COPY . .
WORKDIR "/src/."
RUN dotnet build "ReactAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReactAPI.csproj" -c Release -o /app/publish

# generate runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReactAPI.dll"]