﻿// <auto-generated />
using DAL.EFCore.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DAL.EFCore.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DAL.EFCore.Entities.Device.EfDeviceOption", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("AutoBuild");

                    b.Property<bool>("AutoStart");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ExchangeKeysCollection");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("TopicName4MessageBroker")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("DeviceOptions");
                });

            modelBuilder.Entity("DAL.EFCore.Entities.Exchange.EfExchangeOption", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("AutoStartCycleFunc");

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
