using AutoMapper;
using Domain.Device.Repository.Abstract;
using Domain.Device.Repository.Entities.ResponseProduser;
using Infrastructure.Dal.EfCore;
using Infrastructure.Dal.EfCore.Entities.ResponseProduser;

namespace Domain.Device.Repository.Concrete.EF
{
    public class EfProduserUnionOptionRepository : EfBaseRepository<EfProduserUnionOption, ProduserUnionOption>, IProduserUnionOptionRepository
    {
        public EfProduserUnionOptionRepository(string connectionString, IMapper mapper) : base(connectionString, mapper)
        {
        }
    }
}