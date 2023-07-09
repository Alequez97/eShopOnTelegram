FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/eShopOnTelegram.TelegramBot/eShopOnTelegram.TelegramBot.csproj", "src/eShopOnTelegram.TelegramBot/"]
COPY ["src/eShopOnTelegram.ExternalServices/eShopOnTelegram.ExternalServices.csproj", "src/eShopOnTelegram.ExternalServices/"]
COPY ["src/eShopOnTelegram.Domain/eShopOnTelegram.Domain.csproj", "src/eShopOnTelegram.Domain/"]
COPY ["src/eShopOnTelegram.Persistence/eShopOnTelegram.Persistence.csproj", "src/eShopOnTelegram.Persistence/"]
COPY ["src/eShopOnTelegram.RuntimeConfiguration/eShopOnTelegram.RuntimeConfiguration.csproj", "src/eShopOnTelegram.RuntimeConfiguration/"]
RUN dotnet restore "src/eShopOnTelegram.TelegramBot/eShopOnTelegram.TelegramBot.csproj"
COPY . .
WORKDIR "/src/src/eShopOnTelegram.TelegramBot"
RUN dotnet build "eShopOnTelegram.TelegramBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eShopOnTelegram.TelegramBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShopOnTelegram.TelegramBot.dll"]