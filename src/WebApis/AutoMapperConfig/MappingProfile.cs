﻿using System;
using App.Services.Agregators;
using AutoMapper;
using Domain.Device;
using Domain.Device.Repository.Entities;
using Domain.Device.Repository.Entities.MiddleWareOption;
using Domain.Device.Repository.Entities.ResponseProduser;
using Domain.Exchange;
using Domain.Exchange.Repository.Entities;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Infrastructure.Dal.EfCore.Entities.Device;
using Infrastructure.Dal.EfCore.Entities.Exchange;
using Infrastructure.Dal.EfCore.Entities.ResponseProduser;
using Infrastructure.Dal.EfCore.Entities.Transport;
using Infrastructure.Transport.Http;
using Infrastructure.Transport.SerialPort;
using Infrastructure.Transport.TcpIp;
using WebApiSwc.DTO.JSON.DevicesStateDto;
using WebApiSwc.DTO.JSON.InputTypesDto;
using WebApiSwc.DTO.JSON.OptionsDto.DeviceOption;
using WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption;
using WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption;
using WebApiSwc.DTO.JSON.OptionsDto.TransportOption;
using WebApiSwc.DTO.XML;

namespace WebApiSwc.AutoMapperConfig
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Option 2 Dto mapping
            CreateMap<DeviceOption, DeviceOptionDto>().ReverseMap();
            CreateMap<ExchangeOption, ExchangeOptionDto>().ReverseMap();
            CreateMap<SerialOption, SerialOptionDto>().ReverseMap();
            CreateMap<TcpIpOption, TcpIpOptionDto>().ReverseMap();
            CreateMap<HttpOption, HttpOptionDto>().ReverseMap();
            CreateMap<TransportOption, TransportOptionsDto>().ReverseMap();
            CreateMap<MiddleWareInDataOption, MiddleWareInDataOptionDto>().ReverseMap();
            CreateMap<ProduserUnionOption, ProduserUnionOptionDto>().ReverseMap();
            #endregion


            #region Option 2 EfEntities mapping
            CreateMap<SerialOption, EfSerialOption>().ReverseMap();
            CreateMap<TcpIpOption, EfTcpIpOption>().ReverseMap();
            CreateMap<HttpOption, EfHttpOption>().ReverseMap();
            CreateMap<DeviceOption, EfDeviceOption>().ReverseMap();
            CreateMap<ExchangeOption, EfExchangeOption>().ReverseMap();
            CreateMap<ProduserUnionOption, EfProduserUnionOption>().ReverseMap();
            #endregion


            #region AdInputType xml in Data mapping
            CreateMap<AdInputType4XmlDto, AdInputType>().ConstructUsing(src => new AdInputType(
                src.Id,
                ConvertString2Int(src.ScheduleId),
                ConvertString2Int(src.TrnId),
                Lang.Ru,
                src.TrainNumber,
                src.TrackNumber,
                src.Platform,
                new EventTrain(ConvertString2NullableInt(src.Direction)),
                new TypeTrain(src.TypeName, src.TypeAlias, ConvertString2NullableInt(src.TrainType)),
                new VagonDirection(src.VagonDirection),
                new Station
                {
                    NameRu = src.StartStation,
                    NameEng = src.StartStationENG,
                    NameCh = src.StartStationCH,
                },
                new Station
                {
                    NameRu = src.EndStation,
                    NameEng = src.EndStationENG,
                    NameCh = src.EndStationCH,
                },
                new Station
                {
                    NameRu = src.WhereFrom
                },
                new Station
                {
                    NameRu = src.WhereTo
                },
                new DirectionStation
                {
                    NameRu = src.DirectionStation
                },
                ConvertString2DataTime(src.RecDateTime),
                ConvertString2DataTime(src.SndDateTime),
                ConvertString2DataTime(src.LateTime),
                ConvertString2DataTime(src.ExpectedTime),
                ConvertString2TimeSpan(src.HereDateTime),
                new Addition
                {
                    NameRu = src.Addition,
                    NameEng = src.AdditionENG
                },
                new Note
                {
                    NameRu = src.Note,
                    NameEng = src.NoteENG
                },
                new DaysFollowing
                {
                    NameRu = src.DaysOfGoing,
                    NameAliasRu = src.DaysOfGoingAlias,
                    NameAliasEng = src.DaysOfGoingAliasENG
                }
                )).ForAllMembers(opt => opt.Ignore());
            #endregion


            #region AdInputType json in Data mapping
            CreateMap<AdInputTypeDto, AdInputType>().ConstructUsing(src => new AdInputType(
                src.Id,
                src.ScheduleId,
                src.TrnId,
                src.Lang,
                src.NumberOfTrain,
                src.PathNumber,
                src.Platform,
                src.Event,
                src.TrainType,
                src.VagonDirection,
                src.StationDeparture,
                src.StationArrival,
                src.StationWhereFrom,
                src.StationWhereTo,
                src.DirectionStation,
                src.ArrivalTime,
                src.DepartureTime,
                src.DelayTime,
                src.ExpectedTime,
                src.StopTime,
                src.Addition,
                src.Note,
                src.DaysFollowing
                )).ForAllMembers(opt => opt.Ignore());
            #endregion


            #region DeviceStateDto mapping
            CreateMap<Device<AdInputType>, DeviceStateDto>()
                .ForMember(dest => dest.Option, opt => opt.MapFrom(src => src.Option))
                .ForMember(dest => dest.Exchanges, opt => opt.MapFrom(src => src.Exchanges));

            CreateMap<IExchange<AdInputType>, ExchangeStateDto>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.KeyExchange))
                .ForMember(dest => dest.CycleExchnageStatus, opt => opt.MapFrom(src => src.CycleBehavior.CycleBehaviorState.ToString()))
                .ForMember(dest => dest.AutoStartCycleFunc, opt => opt.MapFrom(src => src.CycleBehavior.CycleFuncOption.AutoStartCycleFunc));
            #endregion
        }


        private static int? ConvertString2NullableInt(string str)
        {
            if (int.TryParse(str, out var r))
                return r;

            return null;
        }


        private static int ConvertString2Int(string str)
        {
            if (int.TryParse(str, out var r))
                return r;

            return 0;
        }


        private static DateTime? ConvertString2DataTime(string str)
        {
            if (DateTime.TryParse(str, out DateTime val))
            {
                return val;
            }
            return null;
        }


        private DateTime? ConvertString2DataTimeMinute(string minute)
        {
            if (int.TryParse(minute, out var val))
            {
                var minuteRes = new DateTime(1998, 04, 30, 0, val, 0);
                return new DateTime();
            }
            return null;
        }


        private static TimeSpan? ConvertString2TimeSpan(string str)
        {
            if (TimeSpan.TryParse(str, out var val))
            {
                return val;
            }
            return null;
        }
    }
}