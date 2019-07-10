using System;
using AutoMapper;
using DAL.Abstract.Entities.Options.Device;
using DAL.Abstract.Entities.Options.Exchange;
using DAL.Abstract.Entities.Options.MiddleWare;
using DAL.Abstract.Entities.Options.Transport;
using DeviceForExchange;
using Exchange.Base;
using InputDataModel.Autodictor.Entities;
using InputDataModel.Autodictor.Model;
using WebApiSwc.DTO.JSON.DevicesStateDto;
using WebApiSwc.DTO.JSON.OptionsDto.DeviceOption;
using WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption;
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
            CreateMap<MiddleWareInDataOption, MiddleWareInDataOptionDto>().ReverseMap();
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
                .ForMember(dest => dest.Stations, opt => opt.MapFrom(src => CreateStations(src)))
                .ForMember(dest => dest.StationsСut, opt => opt.MapFrom(src => CreateStationsCut(src)))
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
                .ForMember(dest => dest.DelayTime, opt => opt.MapFrom(src => ConvertString2DataTime(src.LateTime)))
                .ForMember(dest => dest.ExpectedTime, opt => opt.MapFrom(src => ConvertString2DataTime(src.ExpectedTime) ?? DateTime.MinValue))
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
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.KeyExchange))
                .ForMember(dest => dest.CycleExchnageStatus, opt => opt.MapFrom(src => src.CycleExchnageStatus.ToString()));
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


        private static Station CreateStations(AdInputType4XmlDto dto)
        {
            string CreateStationName(string stArrivalName, string stDepartName)
            {
                stArrivalName = stArrivalName ?? string.Empty;
                stDepartName = stDepartName ?? string.Empty;

                var stations = string.Empty;
                if (!string.IsNullOrEmpty(stArrivalName) && !string.IsNullOrEmpty(stDepartName))
                {
                    stations = $"{stDepartName}-{stArrivalName}";
                }
                return stations;
            }
            var newStation = new Station
            {
                NameRu = CreateStationName(dto.EndStation, dto.StartStation),
                NameEng = CreateStationName(dto.EndStationENG, dto.StartStationENG),
                NameCh = CreateStationName(dto.EndStationCH, dto.StartStationCH)
            };
            return newStation;
        }


        private static Station CreateStationsCut(AdInputType4XmlDto dto)
        {
            string CreateStationCutName(string stArrivalName, string stDepartName)
            {
                stArrivalName = stArrivalName ?? string.Empty;
                stDepartName = stDepartName ?? string.Empty;

                var eventNum = ConvertString2NullableInt(dto.Direction);
                if (!eventNum.HasValue)
                    return string.Empty;

                var stations = string.Empty;
                switch (eventNum.Value)
                {
                    case 0: //"ПРИБ"
                        stations = stDepartName;
                        break;
                    case 1:  //"ОТПР"
                        stations = stArrivalName;
                        break;
                    case 2:   //"СТОЯНКА"
                        stations = $"{stDepartName}-{stArrivalName}";
                        break;
                }
                return stations;
            }

            var newStation = new Station
            {
                NameRu = CreateStationCutName(dto.EndStation, dto.StartStation),
                NameEng = CreateStationCutName(dto.EndStationENG, dto.StartStationENG),
                NameCh = CreateStationCutName(dto.EndStationCH, dto.StartStationCH)
            };
            return newStation;
        }

    }




}