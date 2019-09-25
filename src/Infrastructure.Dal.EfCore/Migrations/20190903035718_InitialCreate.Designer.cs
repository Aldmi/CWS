﻿// <auto-generated />

using Infrastructure.Dal.EfCore.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Dal.EfCore.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20190903035718_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DAL.EFCore.Entities.Device.EfDeviceOption", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("AutoBuild");

                    b.Property<bool>("AutoStart");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ExchangeKeysCollection");

                    b.Property<string>("MiddleWareInDataMetaData");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("ProduserUnionKey")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("DeviceOptions");
                });

            modelBuilder.Entity("DAL.EFCore.Entities.Exchange.EfExchangeOption", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("CycleFuncOptionMetaData")
                        .IsRequired();

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<string>("KeyTransportMetaData")
                        .IsRequired();

                    b.Property<int>("NumberErrorTrying");

                    b.Property<int>("NumberTimeoutTrying");

                    b.Property<string>("ProviderOptionMetaData")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ExchangeOptions");
                });

            modelBuilder.Entity("DAL.EFCore.Entities.Produser.EfProduserUnionOption", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("ConverterName");

                    b.Property<string>("KafkaProduserOptionsMetaData");

                    b.Property<string>("Key");

                    b.Property<string>("SignalRProduserOptionsMetaData");

                    b.Property<string>("WebClientProduserOptionsMetaData");

                    b.HasKey("Id");

                    b.ToTable("ProduserUnionOptions");
                });

            modelBuilder.Entity("DAL.EFCore.Entities.Transport.EfHttpOption", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<bool>("AutoStart");

                    b.Property<string>("HeadersCollection");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("HttpOptions");
                });

            modelBuilder.Entity("DAL.EFCore.Entities.Transport.EfSerialOption", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("AutoStart");

                    b.Property<int>("BaudRate");

                    b.Property<int>("DataBits");

                    b.Property<bool>("DtrEnable");

                    b.Property<int>("Parity");

                    b.Property<string>("Port")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<bool>("RtsEnable");

                    b.Property<int>("StopBits");

                    b.HasKey("Id");

                    b.ToTable("SerialPortOptions");
                });

            modelBuilder.Entity("DAL.EFCore.Entities.Transport.EfTcpIpOption", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("AutoStartBg");

                    b.Property<int>("DutyCycleTimeBg");

                    b.Property<string>("IpAddress")
                        .IsRequired();

                    b.Property<int>("IpPort");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("TcpIpOptions");
                });
#pragma warning restore 612, 618
        }
    }
}