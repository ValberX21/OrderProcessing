FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrderProcessing.Consumer/OrderProcessing.Consumer.csproj", "OrderProcessing.Consumer/"]
RUN dotnet restore "./OrderProcessing.Consumer/OrderProcessing.Consumer.csproj"
COPY . .
WORKDIR "/src/OrderProcessing.Consumer"
RUN dotnet build "./OrderProcessing.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderProcessing.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderProcessing.Consumer.dll"]