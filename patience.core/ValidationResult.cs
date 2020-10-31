namespace patience.core
{
    public enum Status
    {
        Ok
    }
    public class ValidationResult<T>
    {
        public T Data { get; set; }
        public Status Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}