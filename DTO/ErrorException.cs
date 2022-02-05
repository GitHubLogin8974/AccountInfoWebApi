namespace AccountInfoWebApi.DTO
{
    public class ErrorException
    {
        public string ErrorTitle { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
    }
}
