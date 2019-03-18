using System;
using AutoMapper;
using DAL.Abstract.Entities.Options.Device;
using DAL.Abstract.Entities.Options.Exchange;
using DAL.Abstract.Entities.Options.Transport;
using DeviceForExchange;
using Exchange.Base;
using InputDataModel.Autodictor.Entities;
using InputDataModel.Autodictor.Model;
using WebApiSwc.DTO.JSON.DevicesStateDto;
using WebApiSwc.DTO.JSON.OptionsDto.DeviceOption;
using WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption;
using WebApiSwc.DTO.JSON.OptionsDto.TransportOption;
using WebApiSwc.DTO.XML;

namespace WebApiSwc.AutoMapperConfig
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Option mapping
            CreateMap<DeviceOption, DeviceOptionDto>().ReverseMap();
            CreateMap<ExchangeOption, ExchangeOptionDto>().ReverseMap();
            CreateMap<SerialOption, SerialOptionDto>().ReverseMap();
            CreateMap<TcpIpOption, TcpIpOptionDto>().ReverseMap();
            CreateMap<HttpOption, HttpOptionDto>().ReverseMap();
            CreateMap<TransportOption, TransportOptionsDto>().ReverseMap();
            #endregion


            #region AdInputType xml in Data mapping
            CreateMap<AdInputType4XmlDto, AdInputType>()
                .ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => ConvertString2Int(src.ScheduleId)))
                .ForMember(dest => dest.TrnId, opt => opt.MapFrom(src => ConvertString2Int(src.TrnId)))
                .ForMember(dest => dest.Lang, opt => opt.MapFrom(src => Lang.Ru))
                .ForMember(dest => dest.NumberOfTrain, opt => opt.MapFrom(src => src.TrainNumber))
                .ForMember(dest => dest.PathNumber, opt => opt.MapFrom(src => src.TrackNumber))
                .ForMember(dest => dest.Platform, opt => opt.MapFrom(src => src.Platform))
                .ForMember(dest => dest.Event, opt => opt.MapFrom(src => new EventTrain(ConvertString2NullableInt(src.Direction))))
                .ForMember(dest => dest.TrainType, opt => opt.MapFrom(src => new TypeTrain
                {
                    NameRu = src.TypeName,
                    NameAliasRu = src.TypeAlias,
                    Num = ConvertString2NullableInt(src.TrainType) //TODO: игнор
                }))
                .ForMember(dest => dest.VagonDirection, opt => opt.MapFrom(src => new VagonDirection(src.VagonDirection)))
                .ForMember(dest => dest.StationDeparture, opt => opt.MapFrom(src => new Station
                {
                    NameRu = src.StartStation,
                    NameEng = src.StartStationENG,
                    NameCh = src.StartStationCH,
                }))
                .ForMember(dest => dest.StationArrival, opt => opt.MapFrom(src => new Station
                {
                    NameRu = src.EndStation,
                    NameEng = src.EndStationENG,
                    NameCh = src.EndStationCH,
                }))
                .ForMember(dest => dest.StationWhereFrom, opt => opt.MapFrom(src => new Station
                {
                    NameRu = src.WhereFrom
                }))
                .ForMember(dest => dest.StationWhereTo, opt => opt.MapFrom(src => new Station
                {
                    NameRu = src.WhereTo
                }))
                .ForMember(dest => dest.DirectionStation, opt => opt.MapFrom(src => new DirectionStation
                {
                    NameRu = src.DirectionStation
                }))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => ConvertString2DataTime(src.RecDateTime)))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => ConvertString2DataTime(src.SndDateTime)))
                .ForMember(dest => dest.DelayTime, opt => opt.MapFrom(src => ConvertString2DataTimeMinute(src.LateTime)))
                .ForMember(dest => dest.ExpectedTime, opt => opt.MapFrom(src => ConvertString2DataTimeMinute(src.ExpectedTime) ?? DateTime.MinValue))
                .ForMember(dest => dest.StopTime, opt => opt.MapFrom(src => ConvertString2TimeSpan(src.HereDateTime)))
                .ForMember(dest => dest.Addition, opt => opt.MapFrom(src => new Addition
                {
                    NameRu = src.Addition,
                    NameEng = src.AdditionENG
                }))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => new Note
                {
                    NameRu = src.Note,
                    NameEng = src.NoteENG
                }))
                .ForMember(dest => dest.DaysFollowing, opt => opt.MapFrom(src => new DaysFollowing
                {
                    NameRu = src.DaysOfGoing,
                    NameAliasRu = src.DaysOfGoingAlias,
                    NameAliasEng = src.DaysOfGoingAliasENG
                }));
            #endregion


            #region DeviceStateDto mapping
            CreateMap<Device<AdInputType>, DeviceStateDto>()
                .ForMember(dest => dest.Option, opt => opt.MapFrom(src => src.Option))
                .ForMember(dest => dest.Exchanges, opt => opt.MapFrom(src => src.Exchanges));

            CreateMap<IExchange<AdInputType>, ExchangeStateDto>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.KeyExchange));
            #endregion
        }


        private int? ConvertString2NullableInt(string str)
        {
            if (int.TryParse(str, out var r))
                return r;
                    
            return null;
        }

        private int ConvertString2Int(string str)
        {
            if (int.TryParse(str, out var r))
                return r;

            return 0;
        }



        private DateTime? ConvertString2DataTime(string str)
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


        private TimeSpan? ConvertString2TimeSpan(string str)
        {
            if (TimeSpan.TryParse(str, out var val))
            {
                return val;
            }
            return null;
        }
    }




}