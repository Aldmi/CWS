﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Dal.EfCore.Entities.Transport
{
    public class EfTcpIpOption : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
    
        [Required]
        [RegularExpression(@"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)")]
        public string IpAddress { get; set; }         
        
        [Range(0,65535)]
        public int IpPort { get; set; }                 

        public bool AutoStartBg { get; set; }

        public int DutyCycleTimeBg { get; set; } 
    }
}