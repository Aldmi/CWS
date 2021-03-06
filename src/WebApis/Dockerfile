#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WebApis/WebApiSwc.csproj", "WebApis/"]
COPY ["Domain.InputDataModel.OpcServer/Domain.InputDataModel.OpcServer.csproj", "Domain.InputDataModel.OpcServer/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Domain.InputDataModel.Shared/Domain.InputDataModel.Shared.csproj", "Domain.InputDataModel.Shared/"]
COPY ["Infrastructure.Dal.EfCore/Infrastructure.Dal.EfCore.csproj", "Infrastructure.Dal.EfCore/"]
COPY ["Infrastructure.Dal.Abstract/Infrastructure.Dal.Abstract.csproj", "Infrastructure.Dal.Abstract/"]
COPY ["Infrastructure.Storages/Infrastructure.Storages.csproj", "Infrastructure.Storages/"]
COPY ["InputDataModel.Base/Domain.InputDataModel.Base.csproj", "InputDataModel.Base/"]
COPY ["Infrastructure.Transport/Infrastructure.Transport.csproj", "Infrastructure.Transport/"]
COPY ["Infrastructure.Background/Infrastructure.Background.csproj", "Infrastructure.Background/"]
COPY ["Infrastructure/Infrastructure.EventBus.csproj", "Infrastructure/"]
COPY ["InputDataModel.Autodictor/Domain.InputDataModel.Autodictor.csproj", "InputDataModel.Autodictor/"]
COPY ["Device.Base/Domain.Device.csproj", "Device.Base/"]
COPY ["WebClientProduser/Infrastructure.Produser.WebClientProduser.csproj", "WebClientProduser/"]
COPY ["AbstractProduser/Infrastructure.Produser.AbstractProduser.csproj", "AbstractProduser/"]
COPY ["KaffkaProduser/Infrastructure.Produser.KafkaProduser.csproj", "KaffkaProduser/"]
COPY ["Exchange.Base/Domain.Exchange.csproj", "Exchange.Base/"]
COPY ["Infrastructure.MessageBroker/Infrastructure.MessageBroker.csproj", "Infrastructure.MessageBroker/"]
COPY ["BL.Services/App.Services.csproj", "BL.Services/"]
RUN dotnet restore "WebApis/WebApiSwc.csproj"
COPY . .
WORKDIR "/src/WebApis"
RUN dotnet build "WebApiSwc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiSwc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiSwc.dll"]