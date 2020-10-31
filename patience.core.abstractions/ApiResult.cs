namespace patience.core
{
    public enum ApiStatus
    {
        Ok
    }
    public class ApiResult
    {
        public ApiLayout Layout { get; set; }
        public ApiStatus Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}