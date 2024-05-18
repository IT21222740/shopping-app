
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using SendGrid;
using Serilog;
using Application.Services.Interfaces;
using Application.DTOs.Email;
using Application.Services.Helpers;

namespace Infrastructure.EmailService
{
    public class EmailService : IEmailService
    {
        /// <summary>
        /// Email service configurations
        /// </summary>
        private readonly EmailServiceSettings emailServiceSettings;
        private readonly IConfiguration configuration;
        private readonly ILogger<EmailService> logger;
        private readonly ISendGridClient _sendGridClient;

        public EmailService(EmailServiceSettings emailServiceSettings, IConfiguration configuration, ILogger<EmailService> logger, ISendGridClient sendGridClient)
        {
            this.emailServiceSettings = emailServiceSettings;
            this.configuration = configuration;
            this.logger = logger;

            _sendGridClient = sendGridClient;
        }

        public async Task<OperationOutputDTO<EmailResponseDTO>> SendEmailAsync(SendEmailModelDTO sendEmailModel)
        {
            try
            {
                var client = new SendGridClient(emailServiceSettings.SendGridKey);

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(
                               sendEmailModel.SenderEmail,
                               sendEmailModel.SenderName),
                    Subject = sendEmailModel.Subject,
                    TemplateId = sendEmailModel.TemplateId
                };

                msg.SetTemplateData(sendEmailModel.TemplateData);

                foreach (var email in sendEmailModel.ReceiverEmails)
                {
                    msg.AddTo(new EmailAddress(email));
                }

                // Add CC emails
                if (sendEmailModel.CcEmails != null && sendEmailModel.CcEmails.Count > 0)
                {
                    foreach (var ccEmail in sendEmailModel.CcEmails)
                    {
                        if (!sendEmailModel.ReceiverEmails.Contains(ccEmail))
                        {
                            msg.AddCc(ccEmail.Trim());
                        }
                    }
                }

                // Add BCC emails
                if (sendEmailModel.BccEmails != null && sendEmailModel.BccEmails.Count > 0)
                {
                    foreach (var bccEmail in sendEmailModel.BccEmails)
                    {
                        if (!sendEmailModel.ReceiverEmails.Contains(bccEmail) && ((sendEmailModel.CcEmails != null && sendEmailModel.CcEmails.Count > 0 && !sendEmailModel.CcEmails.Contains(bccEmail)) || (sendEmailModel.CcEmails == null || sendEmailModel.CcEmails.Count == 0)))
                        {
                            msg.AddCc(bccEmail.Trim());
                        }
                    }
                }

                // For attachments
                if (sendEmailModel.Attachments != null && sendEmailModel.Attachments.Count > 0)
                {
                    foreach (var item in sendEmailModel.Attachments)
                    {
                        msg.AddAttachment(filename: item.FileName, base64Content: item.Attachment);
                    }
                }

                Log.Information("Sending email to {ReceiverEmails}", string.Join(',', sendEmailModel.ReceiverEmails));

                var response = await _sendGridClient.SendEmailAsync(msg);

                return MapSendGridResponseToDTO(response);

            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "{Message}", CompleteExceptionMessage.Get(ex));

                EmailResponseDTO failedResponse = new EmailResponseDTO
                {
                    Success = false,
                    StatusCode = "500",
                    ErrorMessage = ex.Message
                };

                return OperationOutputDTO<EmailResponseDTO>.Failed(failedResponse.ErrorMessage);
            }
        }

        private OperationOutputDTO<EmailResponseDTO> MapSendGridResponseToDTO(Response response)
        {
            var dto = new EmailResponseDTO
            {
                Success = response.IsSuccessStatusCode,
                StatusCode = response.StatusCode.ToString()
            };

            if (!response.IsSuccessStatusCode)
            {
                // Optionally handle different status codes here
                dto.ErrorMessage = response.Body.ReadAsStringAsync().Result;
            }

            return OperationOutputDTO<EmailResponseDTO>.Success(dto);
        }

    }
}
