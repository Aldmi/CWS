using System;
using InputDataModel.Autodictor.StronglyTypedResponse.Types;
using InputDataModel.Base.Response;
using Shared.Types;

namespace InputDataModel.Autodictor.StronglyTypedResponse
{
    public class AutodictorStronglyTypedResponseFactory : IStronglyTypedResponseFactory
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