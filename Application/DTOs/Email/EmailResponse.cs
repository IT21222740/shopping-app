namespace Application.DTOs.Email
{
    public class EmailResponseDTO
    {
        public bool Success { get; set; }
        public required string StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
