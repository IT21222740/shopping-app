using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Email
{
    public class SendGridAttachmentsDTO
    {
        public string? FileName { get; set; }
        public string? Attachment { get; set; }
    }
}
