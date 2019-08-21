namespace Shared.Types
{
    public interface IStronglyTypedResponseFactory
    {
        StronglyTypedRespBase CreateStronglyTypedResponse(string stronglyTypedName, string response);
    }
}