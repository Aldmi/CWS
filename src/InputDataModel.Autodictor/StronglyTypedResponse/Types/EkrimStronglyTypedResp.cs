using Domain.InputDataModel.Base.Response;

namespace Domain.InputDataModel.Autodictor.StronglyTypedResponse.Types
{
    public class EkrimStronglyTypedResp : StronglyTypedRespBase
    {
        public readonly string Address;

        public EkrimStronglyTypedResp(string response)
        {
            Address = response; //DEBUG
        }


        public override string ToString()
        {
            var resStr = $"{base.ToString()}  Address= {Address}";
            return resStr;
        }
    }
}