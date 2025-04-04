namespace KoneProject.Helpers
{
    public class ApiRessponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; } = null;

        public ApiRessponse(bool success, string message, object? data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
