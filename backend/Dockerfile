FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ./*.props ./

COPY ["src/OrderDeliveryService/OrderDeliveryService.csproj", "src/OrderDeliveryService/"]
COPY ["src/Application/OrderDeliveryService.Application/OrderDeliveryService.Application.csproj", "src/Application/OrderDeliveryService.Application/"]
COPY ["src/Application/OrderDeliveryService.Application.Abstractions/OrderDeliveryService.Application.Abstractions.csproj", "src/Application/OrderDeliveryService.Application.Abstractions/"]
COPY ["src/Application/OrderDeliveryService.Application.Contracts/OrderDeliveryService.Application.Contracts.csproj", "src/Application/OrderDeliveryService.Application.Contracts/"]
COPY ["src/Application/OrderDeliveryService.Application.Models/OrderDeliveryService.Application.Models.csproj", "src/Application/OrderDeliveryService.Application.Models/"]
COPY ["src/Infrastructure/OrderDeliveryService.Infrastructure.Persistence/OrderDeliveryService.Infrastructure.Persistence.csproj", "src/Infrastructure/OrderDeliveryService.Infrastructure.Persistence/"]
COPY ["src/Presentation/OrderDeliveryService.Presentation/OrderDeliveryService.Presentation.csproj", "src/Presentation/OrderDeliveryService.Presentation/"]

RUN dotnet restore "src/OrderDeliveryService/OrderDeliveryService.csproj"

COPY . .
WORKDIR "/src/src/OrderDeliveryService"
RUN dotnet build "OrderDeliveryService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OrderDeliveryService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "OrderDeliveryService.dll"]