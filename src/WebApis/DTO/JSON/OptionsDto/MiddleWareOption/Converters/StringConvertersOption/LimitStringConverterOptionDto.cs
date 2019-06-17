namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    /// <summary>
    /// Обрезка строки, если превышает Limit
    /// </summary>
    public class LimitStringConverterOptionDto
    {
        public int Limit { get; set; }
    }
}