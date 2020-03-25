using System;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreLocalLog.EmailExceptions
{
    public static class ExceptionPrinter
    {
        public static string ExceptionToText(Exception exception, HttpContext context)
        {
            var ret = new StringBuilder();
            AddRequestInfo(context, ret);
            RecursiveExceptionPrinter(exception, ret);
            return ret.ToString();
        }

        private static void AddRequestInfo(HttpContext context, StringBuilder ret)
        {
            ret.Append("Url: ");
            ret.AppendLine(RequestUrl(context.Request));
        }

        private static void RecursiveExceptionPrinter(Exception? exception, StringBuilder ret)
        {
            if (exception == null) return;
            RecursiveExceptionPrinter(exception.InnerException, ret);
            ret.Append("Message: ");
            ret.AppendLine(exception.Message);
            ret.AppendLine(exception.StackTrace);
            ret.AppendLine("-----");
        }

        private static string RequestUrl(HttpRequest request) => string.Concat(
            request.Scheme,
            "://",
            request.Host.ToUriComponent(),
            request.PathBase.ToUriComponent(),
            request.Path.ToUriComponent(),
            request.QueryString.ToUriComponent());

    }
}