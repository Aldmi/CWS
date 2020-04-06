namespace Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.EnumsConvertersOption
{
    /// <summary>
    /// Базовый класс опций для всех конверторов
    /// </summary>
    public class EnumConverterOption
    {
        /// <summary>
        /// Полный путь до типа в сборки.
        /// Вида: "Domain.InputDataModel.Autodictor.Entities.Lang, Domain.InputDataModel.Autodictor"
        /// </summary>
        public string Path2Type { get; set; }
    }
}