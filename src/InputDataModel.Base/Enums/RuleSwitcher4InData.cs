namespace Domain.InputDataModel.Base.Enums
{
    /// <summary>
    /// Выбор Обработчика входных данных.
    /// </summary>
    public enum RuleSwitcher4InData : byte
    {
        None,
        CommandHanler,
        InDataHandler,
        InDataDirectHandler
    }
}