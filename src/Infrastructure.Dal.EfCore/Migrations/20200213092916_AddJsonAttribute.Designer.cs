﻿// <auto-generated />
using Infrastructure.Dal.EfCore.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Dal.EfCore.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20200213092916_AddJsonAttribute")]
    partial class AddJsonAttribute
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Infrastructure.Dal.EfCore.Entities.Device.EfDeviceOption", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<bool>("AutoBuild")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ExchangeKeys")
                        .HasColumnType("text");

                    b.Property<string>("MiddleWareInData")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("ProduserUnionKey")
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("DeviceOptions");
                });

            modelBuilder.Entity("Infrastructure.Dal.EfCore.Entities.Exchange.EfExchangeOption", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("CycleFuncOption")
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("KeyTransport")
                        .HasColumnType("text");

                    b.Property<int>("NumberErrorTrying")
                        .HasColumnType("integer");

                    b.Property<int>("NumberTimeoutTrying")
                        .HasColumnType("integer");

                    b.Property<string>("Provider")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ExchangeOptions");
                });

            modelBuilder.Entity("Infrastructure.Dal.EfCore.Entities.ResponseProduser.EfProduserUnionOption", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("ConverterName")
                        .HasColumnType("text");

                    b.Property<string>("KafkaProduserOptions")
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<string>("SignalRProduserOptions")
                        .HasColumnType("text");

                    b.Property<string>("WebClientProduserOptions")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ProduserUnionOptions");
                });

            modelBuilder.Entity("Infrastructure.Dal.EfCore.Entities.Transport.EfHttpOption", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("AutoStartBg")
                        .HasColumnType("boolean");

                    b.Property<int>("DutyCycleTimeBg")
                        .HasColumnType("integer");

                    b.Property<string>("Headers")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("HttpOptions");
                });

            modelBuilder.Entity("Infrastructure.Dal.EfCore.Entities.Transport.EfSerialOption", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<bool>("AutoStartBg")
                        .HasColumnType("boolean");

                    b.Property<int>("BaudRate")
                        .HasColumnType("integer");

                    b.Property<int>("DataBits")
                        .HasColumnType("integer");

                    b.Property<bool>("DtrEnable")
                        .HasColumnType("boolean");

                    b.Property<int>("DutyCycleTimeBg")
                        .HasColumnType("integer");

                    b.Property<byte>("Parity")
                        .HasColumnType("smallint");

                    b.Property<string>("Port")
                        .IsRequired()
                        .HasColumnType("character varying(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("RtsEnable")
                        .HasColumnType("boolean");

                    b.Property<byte>("StopBits")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("SerialPortOptions");
                });

            modelBuilder.Entity("Infrastructure.Dal.EfCore.Entities.Transport.EfTcpIpOption", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<bool>("AutoStartBg")
                        .HasColumnType("boolean");

                    b.Property<int>("DutyCycleTimeBg")
                        .HasColumnType("integer");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("IpPort")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("TcpIpOptions");
                });
#pragma warning restore 612, 618
        }
    }
}
