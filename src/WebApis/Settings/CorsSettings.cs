using System.Collections.Generic;
using System.Linq;

namespace WebApiSwc.Settings
{
    public class CorsSettings
    {
        public string[] SignalROrigins { get; }
        public string[] WebApiOrigins { get; }



        public CorsSettings(IEnumerable<string> signalROrigins, IEnumerable<string> webApiOrigins)
        {
            SignalROrigins = signalROrigins.ToArray();
            WebApiOrigins = webApiOrigins.ToArray();
        }
    }
}