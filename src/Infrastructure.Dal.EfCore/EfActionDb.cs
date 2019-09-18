using System.Threading.Tasks;
using Infrastructure.Dal.Abstract;
using Infrastructure.Dal.Abstract.Enums;
using Infrastructure.Dal.EfCore.DbContext;

namespace Infrastructure.Dal.EfCore
{
    public class EfActionDb : IActionDb
    {
        private readonly Context _context;


        #region ctor

        public EfActionDb(string connectionString)
        {
            _context = new Context(connectionString);
        }

        #endregion



        public async Task CreateDb(HowCreateDb howCreateDb)
        {
            await _context.CreateDb(howCreateDb);
        }
    }
}