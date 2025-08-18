using Azure.Core;
using System.Net;

namespace AuthService.Models.Generic
{
    public class HttpCustomResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<T> Message { get; set; }
        public string TraceId { get; set; }
    }
}
