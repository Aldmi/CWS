using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers;
using Innofactor.EfCoreJsonValueConverter;
using Shared.Types;

namespace Infrastructure.Dal.EfCore.Entities.StringInsertModelExt
{
    public class EfStringInseartModelExt : IEntity
    {
        public int Id { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Key { get; set; }

        [MaxLength(50)]
        public string Format { get; set; }

        [JsonField]
        public BorderSubString BorderSubString { get; set; }

        [JsonField]
        public EfStringMiddleWareOption StringMiddleWareOption { get; set; }
    }
}