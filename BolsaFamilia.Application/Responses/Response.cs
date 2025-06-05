namespace BolsaFamilia.Application.Responses
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public Response(bool success, string message, T data = default(T), List<string> errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors ?? new List<string>();
        }

        public static Response<T> SuccessResult(T data, string message)
        {
            return new Response<T>(true, message, data);
        }
                
        public static Response<T> FailureResult(string errorMessage)
        {
            return new Response<T>(false, errorMessage);
        }
                
        public static Response<T> FailureResult(string errorMessage, Exception ex)
        {
            return new Response<T>(false, $"{errorMessage} Detalhes: {ex.Message}");
        }
    }
}