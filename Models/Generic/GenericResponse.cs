namespace AuthService.Models.Generic
{
    public class GenericResponse<T>
    {
        public string Message { get; set; }
        //public T Response { get; set; }
        public HttpCustomResponse<T> ResponseMessage { get; set; }
    }
}
