using AutoMapper;
using Domain.InputDataModel.Shared.Repository.Abstract;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Infrastructure.Dal.EfCore;
using Infrastructure.Dal.EfCore.Entities.StringInsertModelExt;

namespace Domain.InputDataModel.Shared.Repository.Concrete.EF
{
    public class EfStringInseartModelExtRepository : EfBaseRepository<EfStringInseartModelExt, StringInsertModelExt>, IStringInsertModelExtRepository
    {
        public EfStringInseartModelExtRepository(string connectionString, IMapper mapper) : base(connectionString, mapper)
        {
        }
    }
}