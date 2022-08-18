using AutoMapper;
using Domain.InputDataModel.Shared.Repository.Abstract;
using Domain.InputDataModel.Shared.StringInseartService.Model.InlineStringInsert;
using Infrastructure.Dal.EfCore;
using Infrastructure.Dal.EfCore.Entities.InlineStringInsertModel;

namespace Domain.InputDataModel.Shared.Repository.Concrete.EF
{
    public class EfInlineStringInsertModelRepository : EfBaseRepository<EfInlineStringInsertModel, InlineStringInsertModel>, IInlineStringInsertModelRepository
    {
        public EfInlineStringInsertModelRepository(string connectionString, IMapper mapper) : base(connectionString, mapper)
        {
        }
    }
}