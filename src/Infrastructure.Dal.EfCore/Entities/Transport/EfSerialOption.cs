﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Dal.EfCore.Entities.Transport
{
    public class EfSerialOption : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Port { get; set; }

        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public EfStopBits StopBits { get; set; }
        public EfParity Parity { get; set; }
        public bool DtrEnable { get; set; }
        public bool RtsEnable { get; set; }

        public bool AutoStartBg { get; set; }
        public int DutyCycleTimeBg { get; set; }
    }


    public enum EfStopBits : byte
    {
        //
        // Summary:
        //     No stop bits are used. This value is not supported by the System.IO.Ports.SerialPort.StopBits
        //     property.
        None = 0,
        //
        // Summary:
        //     One stop bit is used.
        One = 1,
        //
        // Summary:
        //     Two stop bits are used.
        Two = 2,
        //
        // Summary:
        //     1.5 stop bits are used.
        OnePointFive = 3
    }

    public enum EfParity : byte
    {
        //
        // Summary:
        //     No parity check occurs.
        None = 0,
        //
        // Summary:
        //     Sets the parity bit so that the count of bits set is an odd number.
        Odd = 1,
        //
        // Summary:
        //     Sets the parity bit so that the count of bits set is an even number.
        Even = 2,
        //
        // Summary:
        //     Leaves the parity bit set to 1.
        Mark = 3,
        //
        // Summary:
        //     Leaves the parity bit set to 0.
        Space = 4
    }
}