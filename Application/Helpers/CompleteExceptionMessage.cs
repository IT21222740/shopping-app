using System.Text;

namespace Application.Services.Helpers
{
    public static class CompleteExceptionMessage
    {
        public static string Get(this Exception error)
        {
            StringBuilder builder = new StringBuilder();
            Exception realerror = error;
            builder.AppendLine(error.Message);
            while (realerror.InnerException != null)
            {
                builder.AppendLine(realerror.InnerException.Message);
                realerror = realerror.InnerException;
            }
            return builder.ToString();
        }
    }
}
