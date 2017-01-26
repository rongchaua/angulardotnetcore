namespace WebApplication.Models
{
    public class AddResult
    {
        public AddResult(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}