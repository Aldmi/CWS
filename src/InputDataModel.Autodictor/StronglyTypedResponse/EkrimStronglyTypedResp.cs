using System.Net.Sockets;

namespace InputDataModel.Autodictor.StronglyTypedResponse
{
    public class EkrimStronglyTypedResp
    {
        public readonly string Address;

        public EkrimStronglyTypedResp(string response)
        {
            Address = response; //DEBUG
        }
    }
}