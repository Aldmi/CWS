#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

# Set the locale
#RUN apt-get update && apt-get install -y locales
#RUN sed -i -e 's/# ru_RU.UTF-8 UTF-8/ru_RU.UTF-8 UTF-8/' /etc/locale.gen && dpkg-reconfigure --frontend=noninteractive locales && update-locale LANG=ru_RU.UTF-8
ENV LANG ru_RU.UTF-8
ENV LANGUAGE ru_RU:ru
ENV LC_ALL ru_RU.UTF-8

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WebApis/WebApiSwc.csproj", "WebApis/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Infrastructure/Infrastructure.EventBus.csproj", "Infrastructure/"]
COPY ["InputDataModel.Autodictor/Domain.InputDataModel.Autodictor.csproj", "InputDataModel.Autodictor/"]
COPY ["Exchange.Base/Domain.Exchange.csproj", "Exchange.Base/"]
COPY ["InputDataModel.Base/Domain.InputDataModel.Base.csproj", "InputDataModel.Base/"]
COPY ["Infrastructure.Transport/Infrastructure.Transport.csproj", "Infrastructure.Transport/"]
COPY ["Infrastructure.Dal.EfCore/Infrastructure.Dal.EfCore.csproj", "Infrastructure.Dal.EfCore/"]
COPY ["Infrastructure.Dal.Abstract/Infrastructure.Dal.Abstract.csproj", "Infrastructure.Dal.Abstract/"]
COPY ["Infrastructure.Background/Infrastructure.Background.csproj", "Infrastructure.Background/"]
COPY ["Infrastructure.Storages/Infrastructure.Storages.csproj", "Infrastructure.Storages/"]
COPY ["Device.Base/Domain.Device.csproj", "Device.Base/"]
COPY ["WebClientProduser/Infrastructure.Produser.WebClientProduser.csproj", "WebClientProduser/"]
COPY ["AbstractProduser/Infrastructure.Produser.AbstractProduser.csproj", "AbstractProduser/"]
COPY ["KaffkaProduser/Infrastructure.Produser.KafkaProduser.csproj", "KaffkaProduser/"]
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