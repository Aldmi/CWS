using System.Threading.Tasks;
using Infrastructure.Dal.Abstract.Enums;

namespace Infrastructure.Dal.Abstract
{
    /// <summary>
    /// Общие действия с БД.
    /// Создать.
    /// Удалить.
    /// Проверить наличие бд.
    /// </summary>
    public interface IActionDb
    {
        /// <summary>
        /// Создать БД.
        /// </summary>
        Task CreateDb(HowCreateDb howCreateDb);
    }
}