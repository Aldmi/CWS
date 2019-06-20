using System.Net.Mail;

namespace DAL.Abstract.Entities.Options.MiddleWare.Converters
{
    public abstract class BaseConverterOption
    {
        public int Priority { get; set; }
    }
}