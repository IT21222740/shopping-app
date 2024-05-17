//using Application.DTOs;
//using SendGrid.Helpers.Mail;
//using Serilog;
//using System;
//using System.Threading.Tasks;

//namespace Infrastructure.EmailService
//{
//    public class EmailService
//    {
//        public async Task<ServiceResponse> SendEmailAsync(SendEmailModel sendEmailModel)
//        {
//            try
//            {
//                var msg = new SendGridMessage()
//                {
//                    From = new EmailAddress(
//                        sendEmailModel.SenderEmail,
//                        sendEmailModel.SenderName),
//                    Subject = sendEmailModel.Subject,
//                    TemplateId = sendEmailModel.TemplateId
//                };

//                msg.SetTemplateData(sendEmailModel.TemplateData);

//                foreach (var email in sendEmailModel.ReceiverEmails)
//                {
//                    msg.AddTo(new EmailAddress(email));
//                }

//                // Add CC emails
//                if (sendEmailModel.CcEmails != null && sendEmailModel.CcEmails.Count > 0)
//                {
//                    foreach (var ccEmail in sendEmailModel.CcEmails)
//                    {
//                        if (!sendEmailModel.ReceiverEmails.Contains(ccEmail))
//                        {
//                            msg.AddCc(ccEmail.Trim());
//                        }
//                    }
//                }

//                // Add BCC emails
//                if (sendEmailModel.BccEmails != null && sendEmailModel.BccEmails.Count > 0)
//                {
//                    foreach (var bccEmail in sendEmailModel.BccEmails)
//                    {
//                        if (!sendEmailModel.ReceiverEmails.Contains(bccEmail) &&
//                            ((sendEmailModel.CcEmails != null && sendEmailModel.CcEmails.Count > 0 && !sendEmailModel.CcEmails.Contains(bccEmail)) ||
//                            (sendEmailModel.CcEmails == null || sendEmailModel.CcEmails.Count == 0)))
//                        {
//                            msg.AddCc(bccEmail.Trim());
//                        }
//                    }
//                }

//                // For attachments
//                if (sendEmailModel.Attachments != null && sendEmailModel.Attachments.Count > 0)
//                {
//                    foreach (var item in sendEmailModel.Attachments)
//                    {
//                        msg.AddAttachment(filename: item.FileName, base64Content: item.Attachment);
//                    }
//                }

//                Log.Information("Sending email to {ReceiverEmails}", string.Join(',', sendEmailModel.ReceiverEmails));

//                var res = await _sendGridClient.SendEmailAsync(msg);

//                if (res.IsSuccessStatusCode)
//                {
//                    return OperationOutputDTO<Response>.Success(res);
//                }
//                else
//                {
//                    return OperationOutputDTO<Response>.Failed(res.Body.ReadAsStringAsync().Result);
//                }
//            }
//            catch (Exception ex)
//            {
//                logger.LogCritical(ex, "{Message}", CompleteExceptionMessage.Get(ex));
//                return OperationOutputDTO<Response>.Failed(ex.Message);
//            }
//        }
//    }
//}
