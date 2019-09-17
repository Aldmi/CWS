using AutoMapper;
using Infrastructure.Dal.EfCore;
using Infrastructure.Dal.EfCore.Entities.Transport;
using Infrastructure.Transport.Repository.Abstract;
using Infrastructure.Transport.TcpIp;

namespace Infrastructure.Transport.Repository.Concrete.EF
{
    public class EfTcpIpOptionRepository : EfBaseRepository<EfTcpIpOption, TcpIpOption>, ITcpIpOptionRepository
    {
        public EfTcpIpOptionRepository(string connectionString, IMapper mapper) : base(connectionString, mapper)
        {
        }
    }
}