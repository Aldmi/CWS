using System.Collections.Generic;
using System.Linq;
using System.Net;
using Firewall;

namespace WebApiSwc.Settings
{
    public class FirewallSettings
    {
        private readonly IEnumerable<string> _ipAddress;
        private readonly IEnumerable<string> _cidrNotation;


        public List<IPAddress> AllowedIPs => _ipAddress.Select(IPAddress.Parse).ToList();
        public List<CIDRNotation> AllowedCidRs => _cidrNotation.Select(CIDRNotation.Parse).ToList();


        public FirewallSettings(IEnumerable<string> ipAddress, IEnumerable<string> cidrNotation)
        {
            _ipAddress = ipAddress;
            _cidrNotation = cidrNotation;
        }
    }
}