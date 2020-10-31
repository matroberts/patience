namespace patience.core
{
    public enum Status
    {
        Ok
    }
    public class Result
    {
        public Layout Layout { get; set; }
        public Status Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}