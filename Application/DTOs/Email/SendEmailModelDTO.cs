namespace Application.DTOs.Email
{
    public class SendEmailModelDTO
    {
        // Comma seperated receiver emails
        public List<string> ReceiverEmails { get; set; } = new List<string>();

        // Comma seperated cc emails
        public List<string>? CcEmails { get; set; }

        // Comma seperated bcc emails
        public List<string>? BccEmails { get; set; }

        public required string SenderEmail { get; set; }

        public string? SenderName { get; set; }

        public required string Subject { get; set; }

        public required string TemplateId { get; set; }

        public required Dictionary<string, object> TemplateData { get; set; }

        public List<SendGridAttachmentsDTO>? Attachments { get; set; }
    }
}
