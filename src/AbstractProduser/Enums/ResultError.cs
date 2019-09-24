namespace Infrastructure.Produser.AbstractProduser.Enums
{
    public enum ResultError : byte
    {
        Trottling,
        SendException,
        Timeout,
        RespawnProduserError,
        NoClientBySending
    }
}