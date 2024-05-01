using Microsoft.Extensions.Primitives;

namespace GYM_BE.Core.Extentions
{
    public static class HttpRequestExtensions
    {
        public static string? Token(this HttpRequest request)
        {
            foreach (KeyValuePair<string, StringValues> header in request.Headers)
            {
                if (header.Key == "Authorization")
                {
                    try
                    {
                        return header.Value.ToString().Split(" ")[1];
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return null;
        }

    }
}
