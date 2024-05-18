namespace Infrastructure.EmailService
{
    public class EmailServiceSettings
    {
        public required string SenderName { get; set; }
        public required string SenderEmail { get; set; }
        public required string SendGridKey { get; set; }
        public required string WelcomeEmailTemplateId { get; set; }
        public required string PaymentSuccessfulTemplateId { get; set; }

    }

}