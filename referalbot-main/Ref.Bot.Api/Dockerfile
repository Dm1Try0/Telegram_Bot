#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ref.Bot.Api/Ref.Bot.Api.csproj", "Ref.Bot.Api/"]
COPY ["Ref.Bot.Business/Ref.Bot.Business.csproj", "Ref.Bot.Business/"]
COPY ["Ref.Bot.DataAccess/Ref.Bot.DataAccess.csproj", "Ref.Bot.DataAccess/"]
RUN dotnet restore "Ref.Bot.Api/Ref.Bot.Api.csproj"
COPY . .
WORKDIR "/src/Ref.Bot.Api"
RUN dotnet build "Ref.Bot.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ref.Bot.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ref.Bot.Api.dll"]