FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY WebApis/WebApiSwc.csproj WebApis/
COPY Communication.TcpIp/Transport.TcpIp.csproj Communication.TcpIp/
COPY Shared/Shared.csproj Shared/
COPY Transport.Base/Transport.Base.csproj Transport.Base/
COPY DAL.Abstract/DAL.Abstract.csproj DAL.Abstract/
COPY InputDataModel.Autodictor/InputDataModel.Autodictor.csproj InputDataModel.Autodictor/
COPY Exchange.Base/Exchange.Base.csproj Exchange.Base/
COPY InputDataModel.Base/InputDataModel.Base.csproj InputDataModel.Base/
COPY Worker.Background/Worker.Background.csproj Worker.Background/
COPY Infrastructure.MessageBroker1/Infrastructure.MessageBroker.csproj Infrastructure.MessageBroker1/
COPY DAL.EFCore/DAL.EFCore.csproj DAL.EFCore/
COPY BL.Services/BL.Services.csproj BL.Services/
COPY Transport.Http/Transport.Http.csproj Transport.Http/
COPY Communication.SerialPort/Transport.SerialPort.csproj Communication.SerialPort/
COPY Infrastructure/Infrastructure.EventBus.csproj Infrastructure/
COPY Device.Base/DeviceForExchange.csproj Device.Base/
COPY DAL.InMemory/DAL.InMemory.csproj DAL.InMemory/
COPY Option.Test/Option.Test.csproj Option.Test/
RUN dotnet restore WebApis/WebApiSwc.csproj
RUN dotnet restore Option.Test/Option.Test.csproj
COPY . .

WORKDIR /src/Option.Test
RUN dotnet test Option.Test.csproj

WORKDIR /src/WebApis
RUN dotnet build WebApiSwc.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebApiSwc.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApiSwc.dll"]
