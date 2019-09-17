using AutoMapper;
using Infrastructure.Dal.EfCore;
using Infrastructure.Dal.EfCore.Entities.Transport;
using Infrastructure.Transport.Repository.Abstract;
using Infrastructure.Transport.SerialPort;

namespace Infrastructure.Transport.Repository.Concrete.EF
{
    public class EfSerialPortOptionRepository : EfBaseRepository<EfSerialOption, SerialOption>, ISerialPortOptionRepository
    {
        public EfSerialPortOptionRepository(string connectionString, IMapper mapper) : base(connectionString, mapper)
        {
        }
    }
}