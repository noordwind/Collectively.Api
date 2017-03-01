using System;
using Nancy;

namespace Collectively.Api.Framework
{
    public static class Extensions
    {
        public static Response WithResourceIdHeader(this Response response, Guid resourceId)
        {
            return resourceId == Guid.Empty ? response : response.WithHeader("X-Resource-Id", resourceId.ToString("N"));
        }

        public static Response WithLocation(this IResponseFormatter response, string path,
            HttpStatusCode statusCode = HttpStatusCode.Created)
        {
            return response.AsRedirect(path).WithStatusCode(statusCode);
        }
    }
}