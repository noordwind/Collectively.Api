using System;
using System.Collections.Generic;
using Collectively.Common.Extensions;
using Collectively.Messages.Commands.Remarks;

namespace Collectively.Api.Validation
{
    public class ResolveRemarkValidator : IValidator<ResolveRemark>
    {
        private readonly FeatureSettings _featureSettings;

        public ResolveRemarkValidator(FeatureSettings featureSettings)
        {
            _featureSettings = featureSettings;
        }

        public IEnumerable<string> SetPropertiesAndValidate(ResolveRemark value)
        {
            if (value.UserId.Empty())
                yield return "User was not provided.";
            if (value.RemarkId == Guid.Empty)
                yield return "Remark id to be resolved was not provided.";

            foreach (var error in ValidatePhoto(value))
            {
                yield return error;
            }
            foreach (var error in ValidateLocation(value))
            {
                yield return error;
            }
        }

        private IEnumerable<string> ValidatePhoto(ResolveRemark command)
        {
            command.ValidatePhoto = _featureSettings.ResolveRemarkPhotoRequired;
            if (!command.ValidatePhoto)
                yield break;

            var photo = command.Photo;
            if (photo == null)
            {
                yield return "Photo was not provided.";
                yield break;
            }
            if (photo.Base64.Empty())
                yield return "Photo base64 encoding was not provided.";
            if (photo.ContentType.Empty())
                yield return "Photo content type not provided.";
            if (photo.Name.Empty())
                yield return "Photo name was not provided.";
        }

        private IEnumerable<string> ValidateLocation(ResolveRemark command)
        {
            command.ValidateLocation = _featureSettings.ResolveRemarkLocationRequired;
            if (!command.ValidateLocation)
                yield break;
            if (command.Latitude > 90 || command.Latitude < -90)
                yield return "Invalid latitude.";
            if (command.Longitude > 180 || command.Longitude < -180)
                yield return "Invalid longitude.";
        }
    }
}