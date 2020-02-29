using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Dal.EfCore.Entities.Exchange.ProvidersOption;
using Innofactor.EfCoreJsonValueConverter;
using Newtonsoft.Json;
using Shared.Collections;
using Shared.Enums;

namespace Infrastructure.Dal.EfCore.Entities.Exchange
{
    public class EfExchangeOption : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        public int NumberErrorTrying { get; set; }
        public int NumberTimeoutTrying { get; set; }

        [JsonField]
        public EfKeyTransport KeyTransport { get; set; }

        [JsonField]
        public EfProviderOption Provider { get; set; }

        [JsonField]
        public EfCycleFuncOption CycleFuncOption { get; set; }
    }

    public class EfKeyTransport
    {
        #region prop

        public string Key { get; set; }
        public TransportType TransportType { get; set; }

        #endregion
    }

    public class EfCycleFuncOption
    {
        public bool AutoStartCycleFunc { get; set; }
        public int SkipInterval { get; set; }
        public int NormalIntervalCycleDataEntry { get; set; }
        public QueueMode CycleQueueMode { get; set; }
    }
}