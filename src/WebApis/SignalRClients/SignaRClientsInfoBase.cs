namespace WebApiSwc.SignalRClients
{
    public abstract class SignaRClientsInfoBase
    {
        public string ClientId { get; }
        public string GroupName { get; }
        public string UserName { get; }


        protected SignaRClientsInfoBase(string clientId, string groupName)
        {
            ClientId = clientId;
            GroupName = groupName;
        }
    }


    /// <summary>
    /// Клиент SignalR для Produser.
    /// </summary
    public class SignaRProdusserClientsInfo : SignaRClientsInfoBase
    {
        public SignaRProdusserClientsInfo(string clientId, string groupName) : base(clientId, groupName)
        {
        }
    }

}