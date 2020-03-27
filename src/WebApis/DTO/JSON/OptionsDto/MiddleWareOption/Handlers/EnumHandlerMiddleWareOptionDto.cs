using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.EnumsConvertersOption;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.EnumsConvertersOption;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers
{
    public class EnumHandlerMiddleWareOptionDto
    {
        public string PropName { get; set; }    

        public EnumerateConverterOptionDto EnumerateConverterOption { get; set; }

    }
}