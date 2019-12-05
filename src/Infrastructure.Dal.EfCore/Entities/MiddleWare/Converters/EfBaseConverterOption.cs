namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters
{
    public abstract class EfBaseConverterOption
    {
        public int Priority { get; set; }
    }
}