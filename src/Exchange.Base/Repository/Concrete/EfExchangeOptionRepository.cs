using AutoMapper;
using Domain.Exchange.Repository.Abstract;
using Domain.Exchange.Repository.Entities;
using Infrastructure.Dal.EfCore;
using Infrastructure.Dal.EfCore.Entities.Exchange;


namespace Domain.Exchange.Repository.Concrete
{
    public class EfExchangeOptionRepository : EfBaseRepository<EfExchangeOption, ExchangeOption>, IExchangeOptionRepository
    {
        public EfExchangeOptionRepository(string connectionString, IMapper mapper) : base(connectionString, mapper)
        {
        }
    }
}