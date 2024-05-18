namespace Application.DTOs.Email
{
    public class OperationResultDTO
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public string? MessageCode { get; set; }
        public StatusCode StatusCode { get; set; }
        public string? CurrentUserEmail { get; set; }
        public string? ManagerEmail { get; set; }

    }

    public enum StatusCode
    {
        Ok = 200,
        BadRequest = 400,
        NotFound = 404,
        InternalServerError = 500,
        Unauthorized = 401
    }
}
