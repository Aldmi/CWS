using System.Collections.Generic;

namespace Domain.InputDataModel.Base
{
    public interface IIndependentInserts
    {
        Dictionary<string, object> CreateDictionary(object inData);
    }
}