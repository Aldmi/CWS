using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Innofactor.EfCoreJsonValueConverter;

namespace Infrastructure.Dal.EfCore.Entities.Transport
{
    public class EfHttpOption : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Address { get; set; }

        public bool AutoStartBg { get; set; }
        public int DutyCycleTimeBg { get; set; }

        [JsonField]
        public Dictionary<string, string> Headers { get; set; }
    }
}