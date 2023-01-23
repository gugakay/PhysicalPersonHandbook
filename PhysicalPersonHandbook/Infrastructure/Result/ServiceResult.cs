namespace PhysicalPersonHandbook.Infrastructure.Result
{
    public class ServiceResult
    {
        public ServiceResult(bool isSuccess, string message, dynamic data = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
