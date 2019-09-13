FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

# Set the locale
RUN apt-get update && apt-get install -y locales
RUN sed -i -e 's/# ru_RU.UTF-8 UTF-8/ru_RU.UTF-8 UTF-8/' /etc/locale.gen && dpkg-reconfigure --frontend=noninteractive locales && update-locale LANG=ru_RU.UTF-8
ENV LANG ru_RU.UTF-8
ENV LANGUAGE ru_RU:ru
ENV LC_ALL ru_RU.UTF-8

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["WebApis/WebApiSwc.csproj", "WebApis/"]
COPY ["DAL.EFCore/DAL.EFCore.csproj", "DAL.EFCore/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["DAL.Abstract/DAL.Abstract.csproj", "DAL.Abstract/"]
COPY ["AbstractProduser/AbstractProduser.csproj", "AbstractProduser/"]
COPY ["KaffkaProduser/KafkaProduser.csproj", "KaffkaProduser/"]
COPY ["WebClientProduser/WebClientProduser.csproj", "WebClientProduser/"]
COPY ["Infrastructure/Infrastructure.EventBus.csproj", "Infrastructure/"]
COPY ["InputDataModel.Autodictor/Domain.InputDataModel.Autodictor.csproj", "InputDataModel.Autodictor/"]
COPY ["Exchange.Base/Domain.Exchange.csproj", "Exchange.Base/"]
COPY ["InputDataModel.Base/Domain.InputDataModel.Base.csproj", "InputDataModel.Base/"]
COPY ["Infrastructure.Transport/Infrastructure.Transport.csproj", "Infrastructure.Transport/"]
COPY ["Infrastructure.Background/Infrastructure.Background.csproj", "Infrastructure.Background/"]
COPY ["Infrastructure.Storages/Infrastructure.Storages.csproj", "Infrastructure.Storages/"]
COPY ["Device.Base/Domain.Device.csproj", "Device.Base/"]
COPY ["Infrastructure.MessageBroker/Infrastructure.MessageBroker.csproj", "Infrastructure.MessageBroker/"]
COPY ["BL.Services/App.Services.csproj", "BL.Services/"]
COPY ["DAL.InMemory/DAL.InMemory.csproj", "DAL.InMemory/"]
RUN dotnet restore "WebApis/WebApiSwc.csproj"
COPY . .
WORKDIR "/src/WebApis"
RUN dotnet build "WebApiSwc.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebApiSwc.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApiSwc.dll"]