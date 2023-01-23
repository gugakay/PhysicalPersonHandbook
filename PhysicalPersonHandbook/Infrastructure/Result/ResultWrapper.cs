using System.Net;

namespace PhysicalPersonHandbook.Infrastructure.Result
{
    public class ResultWrapper
    {
        public ResultWrapper(HttpStatusCode code, ServiceResult result)
        {
            Data = result;
            StatusCode = code;
        }
        
        public ServiceResult Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
