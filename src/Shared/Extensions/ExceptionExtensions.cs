using System;
using System.Text;

namespace Shared.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetUnionMessages(this Exception ex)
        {
            var sb = new StringBuilder();
            for (var e = ex; e != null; e = e.InnerException)
            {
                var message = e.Message ?? string.Empty;
                sb.AppendLine(message);
            }
            return sb.ToString();
        }


        public static Exception GetOriginalException(this Exception ex)
        {
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex;
        }
    }
}