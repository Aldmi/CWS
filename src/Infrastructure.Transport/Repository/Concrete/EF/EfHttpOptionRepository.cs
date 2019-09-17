using AutoMapper;
using Infrastructure.Dal.EfCore;
using Infrastructure.Dal.EfCore.Entities.Transport;
using Infrastructure.Transport.Http;
using Infrastructure.Transport.Repository.Abstract;

namespace Infrastructure.Transport.Repository.Concrete.EF
{
    public class EfHttpOptionRepository : EfBaseRepository<EfHttpOption, HttpOption>, IHttpOptionRepository
    {
        public EfHttpOptionRepository(string connectionString, IMapper mapper) : base(connectionString, mapper)
        {
        }
    }
}