using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Dal.EfCore.Entities.MiddleWare;
using Innofactor.EfCoreJsonValueConverter;
using Shared.Types;

namespace Infrastructure.Dal.EfCore.Entities.StringInsertModelExt
{
    public class EfStringInseartModelExt : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string VarName { get; set; }

        [MaxLength(50)]
        public string Format { get; set; }

        [JsonField]
        public BorderSubString BorderSubString { get; set; }
    }
}