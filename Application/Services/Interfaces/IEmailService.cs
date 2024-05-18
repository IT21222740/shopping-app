using Application.DTOs.Email;

namespace Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<OperationOutputDTO<EmailResponseDTO>> SendEmailAsync(SendEmailModelDTO sendEmailModel);
    }
}
