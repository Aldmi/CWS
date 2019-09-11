using System;
using InputDataModel.Autodictor.StronglyTypedResponse.Types;
using InputDataModel.Base.Response;

namespace InputDataModel.Autodictor.StronglyTypedResponse
{
    public class AdStronglyTypedResponseFactory : IStronglyTypedResponseFactory
    {
        public StronglyTypedRespBase CreateStronglyTypedResponse(string stronglyTypedName, string response)
        {
            switch (stronglyTypedName)
            {
                case "Ekrim":
                    return new EkrimStronglyTypedResp(response);

                default:
                    throw new NotSupportedException($"Фабрикой строго типизированных ответов Не поддерживает {stronglyTypedName}");
            }
        }
    }
}