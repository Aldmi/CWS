namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    /// <summary>
    /// Обрезка строки, если превышает Limit
    /// </summary>
    public class LimitStringConverterOptionDto : BaseConverterOptionDto
    {
        public int Limit { get; set; }
    }
}