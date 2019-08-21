namespace InputDataModel.Base.Response
{
    public interface IStronglyTypedResponseFactory
    {
        StronglyTypedRespBase CreateStronglyTypedResponse(string stronglyTypedName, string response);
    }
}