using DAL.Abstract.Abstract;
using DAL.Abstract.Entities.Options.Transport;

namespace DAL.Abstract.Concrete
{
    /// <summary>
    /// Доступ к транспорту послед. порты
    /// </summary>
    public interface ISerialPortOptionRepository : IGenericDataRepository<SerialOption>
    {  
    }

}