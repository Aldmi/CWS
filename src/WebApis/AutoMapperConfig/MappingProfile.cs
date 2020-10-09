using System;
using System.Globalization;
using App.Services.Agregators;
using AutoMapper;
using Domain.Device;
using Domain.Device.MiddleWares4InData;
using Domain.Device.Repository.Entities;
using Domain.Device.Repository.Entities.ResponseProduser;
using Domain.Exchange;
using Domain.Exchange.Repository.Entities;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Infrastructure.Dal.EfCore.Entities.Device;
using Infrastructure.Dal.EfCore.Entities.Exchange;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers;
using Infrastructure.Dal.EfCore.Entities.ResponseProduser;
using Infrastructure.Dal.EfCore.Entities.StringInsertModelExt;
using Infrastructure.Dal.EfCore.Entities.Transport;
using Infrastructure.Transport.Http;
using Infrastructure.Transport.SerialPort;
using Infrastructure.Transport.TcpIp;
using Shared.Mathematic;
using Shared.MiddleWares.HandlersOption;
using WebApiSwc.DTO.JSON.DevicesStateDto;
using WebApiSwc.DTO.JSON.InputTypesDto;
using WebApiSwc.DTO.JSON.OptionsDto.DeviceOption;
using WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption;
using WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption;
using WebApiSwc.DTO.JSON.OptionsDto.TransportOption;
using WebApiSwc.DTO.JSON.Shared;
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
            CreateMap<MiddleWareMediatorOption, MiddleWareInDataOptionDto>().ReverseMap();
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


            #region AdInputType xml in ProcessedItemsInBatch mapping
            CreateMap<AdInputType4XmlDto, AdInputType>().ConstructUsing(src => new AdInputType(
                src.Id,
                ConvertString2Int(src.ScheduleId),
                ConvertString2Int(src.TrnId),
                Lang.Ru,                             //DEBUG
                src.TrainNumber,
                src.TrackNumber,
                src.Platform,
                new EventTrain(ConvertString2NullableInt(src.Direction)),
                new TypeTrain(
                    src.TypeName,
                    src.TypeAlias,
                    String.Empty,
                    src.TypeAliasEng,
                    ConvertString2NullableInt(src.TrainType)
                    ),
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
                    NameEng = src.NoteEng
                },
                new DaysFollowing
                {
                    NameRu = src.DaysOfGoing,
                    NameAliasRu = src.DaysOfGoingAlias,
                    NameAliasEng = src.DaysOfGoingAliasENG
                },
                new Emergency(src.EmergencySituation),
                new Category(src.TypeName),
                new CreepingLine("Бегущая строка слово1 слово2 слово3 слово4", null, TimeSpan.FromSeconds(20))
                )).ForAllMembers(opt => opt.Ignore());
            #endregion


            #region AdInputType json in ProcessedItemsInBatch mapping
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
                src.DaysFollowing,
                src.Emergency,
                src.Category,
                src.CreepingLine
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


            #region StringInsertModelExt  mapping

            //CreateMap<MathematicFormulaDto, MathematicFormula>()
            //    .ConstructUsing((src, context) => new MathematicFormula(src.Expr));

            //CreateMap<MathematicFormula, MathematicFormulaDto>();

            //CreateMap<EfMathematicFormula, MathematicFormula>()
            //    .ConstructUsing((src, context) => new MathematicFormula(src.Expr));

            //CreateMap<MathematicFormula, EfMathematicFormula>();
            
            CreateMap<StringInsertModelExt, StringInsertModelExtDto>().ReverseMap();

            CreateMap<StringInsertModelExtDto, StringInsertModelExt>()
                .ConstructUsing((src, context) => new StringInsertModelExt(
                    src.Key,
                    src.Format,
                    src.BorderSubString,
                    context.Mapper.Map<StringHandlerMiddleWareOption>(src.StringHandlerMiddleWareOption),
                    context.Mapper.Map<MathematicFormula>(src.MathematicFormula)
                )).ForAllMembers(opt => opt.Ignore());

            CreateMap<StringInsertModelExt, EfStringInseartModelExt>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.Format, opt => opt.MapFrom(src => src.Format))
                .ForMember(dest => dest.BorderSubString, opt => opt.MapFrom(src => src.BorderSubString))
                .ForMember(dest => dest.StringHandlerMiddleWareOption, opt => opt.MapFrom(src => src.StringHandlerMiddleWareOption))
                .ForMember(dest => dest.MathematicFormula, opt => opt.MapFrom(src => src.MathematicFormula));

            CreateMap<EfStringInseartModelExt, StringInsertModelExt>()
              .ConstructUsing((src, context) => new StringInsertModelExt(
                  src.Key,
                  src.Format,
                  src.BorderSubString,
                 context.Mapper.Map<StringHandlerMiddleWareOption>(src.StringHandlerMiddleWareOption),
                  context.Mapper.Map<MathematicFormula>(src.MathematicFormula)
              )).ForAllMembers(opt => opt.Ignore());
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
            if (double.TryParse(str, out var min))
            {
                var minuteVal = TimeSpan.FromMinutes(min);
                return minuteVal;
            }
            if (TimeSpan.TryParse(str, out var val))
            {
                return val;
            }
            return null;
        }
    }
}