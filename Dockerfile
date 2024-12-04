FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["src/Web/Web.csproj", "src/Web/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
RUN dotnet restore "src/Web/Web.csproj"

COPY . ../
WORKDIR /src/Web
RUN dotnet build "Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GerPros_Backend_API.Web.dll"]


## Development image
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev
#WORKDIR /src
#EXPOSE 8080
#EXPOSE 8081
#
### Copy csproj and restore as distinct layers
#COPY ["src/Web/Web.csproj", "src/Web/"]
#COPY ["src/Application/Application.csproj", "src/Application/"]
#COPY ["src/Domain/Domain.csproj", "src/Domain/"]
#COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
#COPY ["Directory.Build.props", "./"]
#COPY ["Directory.Packages.props", "./"]
#RUN dotnet restore "src/Web/Web.csproj"
#
## Copy everything else and build
#COPY . ../
#WORKDIR /src/Web
#
#RUN dotnet tool install -g dotnet-watch
#
#ENV PATH="$PATH:/root/.dotnet/tools"
#
## Install vsdbg for debugging
#RUN apt-get update && apt-get install -y unzip curl \
#    && mkdir -p /vsdbg \
#    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg
#
#EXPOSE 8080 
#EXPOSE 4020  
#
### Set the entrypoint to use dotnet watch
#ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8080", "--configuration", "Debug"]
