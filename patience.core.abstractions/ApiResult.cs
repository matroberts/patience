namespace patience.core
{
    public enum ApiStatus
    {
        Ok,
        Error
    }
    public class ApiResult
    {
        public ApiLayout Layout { get; set; }
        public ApiStatus Status { get; set; }
        public string Message { get; set; }
    }
}