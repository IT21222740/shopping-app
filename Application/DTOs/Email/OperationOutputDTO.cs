using Microsoft.Extensions.Logging;


namespace Application.DTOs.Email
{
    public class OperationOutputDTO<TOutput> : OperationResultDTO
    {
        public TOutput? Value { get; set; }

        public static OperationOutputDTO<TOutput> Success(TOutput result)
        {
            return new OperationOutputDTO<TOutput>
            {
                Status = true,
                Value = result,
                StatusCode = StatusCode.Ok
            };
        }

        public static OperationOutputDTO<TOutput> Failed(string message, StatusCode statusCode = StatusCode.InternalServerError)
        {
            return new OperationOutputDTO<TOutput>
            {
                MessageCode = string.Empty,
                Message = message,
                StatusCode = statusCode
            };
        }

    }
}
