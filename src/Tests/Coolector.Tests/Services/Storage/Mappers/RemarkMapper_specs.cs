using System;
using System.Dynamic;
using Coolector.Dto.Remarks;
using Coolector.Services.Storage.Mappers;
using FluentAssertions;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Storage.Mappers
{
    [Subject("RemarkMapper Map")]
    public class when_invoking_remark_mapper_map : Mapper_specs<RemarkDto>
    {
        private Establish context = () =>
        {
            dynamic author = new ExpandoObject();
            author.userId = Guid.NewGuid().ToString();
            author.name = "user1";

            dynamic category = new ExpandoObject();
            category.id = Guid.NewGuid();
            category.name = "litter";

            dynamic photo = new ExpandoObject();
            photo.fileId = Guid.NewGuid().ToString();
            photo.name = "file.png";
            photo.contentType = "image/png";

            dynamic location = new ExpandoObject();
            location.address = "test";
            location.coordinates = new[] {1d, 2d};
            location.type = "Point";

            Initialize(new RemarkMapper());
            Source.id = Guid.NewGuid();
            Source.author = author;
            Source.category = category;
            Source.photo = photo;
            Source.location = location;
            Source.description = "test";
            Source.resolved = true;
            Source.resolvedAt = DateTime.UtcNow;
            Source.createdAt = DateTime.UtcNow;
        };

        Because of = () => Map();

        It should_map_remark_object = () =>
        {
            Result.ShouldNotBeNull();
            Result.Id.ShouldBeEquivalentTo((Guid)Source.id);
            Result.Author.UserId.ShouldBeEquivalentTo((string)Source.author.userId);
            Result.Author.Name.ShouldBeEquivalentTo((string)Source.author.name);
            Result.Category.Id.ShouldBeEquivalentTo((Guid)Source.category.id);
            Result.Category.Name.ShouldBeEquivalentTo((string)Source.category.name);
            Result.Photo.FileId.ShouldBeEquivalentTo((string)Source.photo.fileId);
            Result.Photo.Name.ShouldBeEquivalentTo((string)Source.photo.name);
            Result.Photo.ContentType.ShouldBeEquivalentTo((string)Source.photo.contentType);
            Result.Location.Address.ShouldBeEquivalentTo((string)Source.location.address);
            Result.Location.Coordinates[0].ShouldBeEquivalentTo((double)Source.location.coordinates[0]);
            Result.Location.Coordinates[1].ShouldBeEquivalentTo((double)Source.location.coordinates[1]);
            Result.Location.Type.ShouldBeEquivalentTo((string)Source.location.type);
            Result.Description.ShouldBeEquivalentTo((string)Source.description);
            Result.Resolved.ShouldBeEquivalentTo((bool)Source.resolved);
            Result.ResolvedAt.ShouldBeEquivalentTo((DateTime)Source.resolvedAt);
            Result.CreatedAt.ShouldBeEquivalentTo((DateTime)Source.createdAt);
        };
    }
}