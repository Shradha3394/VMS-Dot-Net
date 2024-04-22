namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class ApplicationResponseDTO<T>
    {
        public Error? Error { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class Error
    {
        public int Code { get; set; }
        public List<String> Message { get; set; } = new List<string>();
    }
}
