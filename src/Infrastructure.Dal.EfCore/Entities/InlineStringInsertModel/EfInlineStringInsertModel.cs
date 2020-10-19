using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Dal.EfCore.Entities.InlineStringInsertModel
{
    public class EfInlineStringInsertModel : IEntity
    {
        public int Id { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Key { get; set; }

        public string InlineStr { get; set; }

        public string Description { get; set; }
    }
}