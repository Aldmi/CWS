
using AutoMapper;
using Domain.Device.Repository.Abstract;
using Domain.Device.Repository.Entities;
using Infrastructure.Dal.EfCore;
using Infrastructure.Dal.EfCore.Entities.Device;

namespace Domain.Device.Repository.Concrete.EF
{
    public class EfDeviceOptionRepository : EfBaseRepository<EfDeviceOption, DeviceOption>, IDeviceOptionRepository
    {
        public EfDeviceOptionRepository(string connectionString, IMapper mapper) : base(connectionString, mapper)
        {
        }
    }
}