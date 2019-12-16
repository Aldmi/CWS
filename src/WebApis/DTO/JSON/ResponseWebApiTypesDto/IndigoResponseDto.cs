namespace WebApiSwc.DTO.JSON.ResponseWebApiTypesDto
{
    public class IndigoResponseDto
    {
        public int Result { get;  }
        public string Message { get; }

        public IndigoResponseDto(int result, string message)
        {
            Result = result;
            Message = message;
        }
    }
}