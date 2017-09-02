using System;
using System.Collections.Generic;
using Collectively.Api.Queries;
using Collectively.Common.Locations;
using Collectively.Services.Storage.Models.Remarks;
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

        public static bool IsLocationProvided(this BrowseRemarksBase query)
        => (Math.Abs(query.Latitude) <= 0.0000000001 || 
            Math.Abs(query.Longitude) <= 0.0000000001) == false;

        public static void SetRemarksDistance(this IEnumerable<Remark> remarks, double latitude, double longitude)
        {
            var center = new Coordinates(latitude, longitude);
            foreach (var remark in remarks)
            {
                var coordinates = new Coordinates(remark.Location.Latitude, remark.Location.Longitude);
                remark.Distance = center.DistanceTo(coordinates, UnitOfLength.Meters);
            }
        }
    }
}