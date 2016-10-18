using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Coolector.Dto.Remarks;
using Coolector.Tests.EndToEnd.Framework;
using FluentAssertions;
using Machine.Specifications;

namespace Coolector.Tests.EndToEnd.API.Modules
{
    public abstract class RemarksModule_specs : ModuleBase_specs
    {
        protected static IPhotoGenerator PhotoGenerator = new PhotoGenerator(); 

        protected static RemarkDto GetRemark(Guid id)
            => HttpClient.GetAsync<RemarkDto>($"remarks/{id}").WaitForResult();

        protected static IEnumerable<RemarkDto> GetLatestRemarks()
            => HttpClient.GetCollectionAsync<RemarkDto>("remarks?latest=true").WaitForResult();

        protected static IEnumerable<RemarkCategoryDto> GetCategories()
            => HttpClient.GetCollectionAsync<RemarkCategoryDto>("remarks/categories").WaitForResult();

        protected static Stream GetPhoto(Guid id)
            => HttpClient.GetStreamAsync($"remarks/{id}/photo").WaitForResult();

        protected static HttpResponseMessage CreateRemark()
        {
            var categories = GetCategories();
            var photo = GeneratePhoto();

            return HttpClient.PostAsync("remarks", new
            {
                Address = "",
                CategoryId = categories.First().Id,
                Description = "test",
                Latitude = 1.0,
                Longitude = 1.0,
                Photo = photo
            }).WaitForResult();
        }

        protected static HttpResponseMessage DeleteRemark(Guid remarkId)
            => HttpClient.DeleteAsync($"remarks/{remarkId}").WaitForResult();

        protected static HttpResponseMessage ResolveRemark(Guid remarkId, double latitude = 1.0, double longitude = 1.0)
            => HttpClient.PutAsync("remarks", new
            {
                RemarkId = remarkId,
                Photo = GeneratePhoto(),
                Latitude = latitude,
                Longitude = longitude
            }).WaitForResult();

        protected static object GeneratePhoto() => PhotoGenerator.GetDefault();
    }

    [Subject("Remarks collection")]
    public class when_fetching_latest_remarks : RemarksModule_specs
    {
        static IEnumerable<RemarkDto> Remarks;

        Establish context = () =>
        {
            Initialize(true);
            CreateRemark();
            Wait();
        };

        Because of = () => Remarks = GetLatestRemarks();

        It should_return_non_empty_collection = () =>
        {
            Remarks.ShouldNotBeEmpty();
            foreach (var remark in Remarks)
            {
                remark.Id.ShouldNotEqual(Guid.Empty);
                remark.Author.UserId.ShouldNotBeEmpty();
                remark.Author.Name.ShouldNotBeEmpty();
                remark.Category.Id.ShouldNotEqual(Guid.Empty);
                remark.Category.Name.ShouldNotBeEmpty();
                remark.Location.Coordinates.Length.ShouldEqual(2);
                remark.Location.Coordinates[0].ShouldNotEqual(0);
                remark.Location.Coordinates[1].ShouldNotEqual(0);
                remark.Photo.Name.ShouldNotBeEmpty();
                remark.Photo.FileId.ShouldNotBeEmpty();
                remark.Photo.ContentType.ShouldNotBeEmpty();
            }
        };
    }

    [Subject("Remark details")]
    public class when_fetching_remark : RemarksModule_specs
    {
        static IEnumerable<RemarkDto> Remarks;
        static RemarkDto SelectedRemark;
        static RemarkDto Remark;
        static Stream Photo;

        Establish context = () =>
        {
            Initialize(true);
            CreateRemark();
            Wait();
        };

        Because of = () =>
        {
            Remarks = GetLatestRemarks();
            SelectedRemark = Remarks.First();
            Remark = GetRemark(SelectedRemark.Id);
            Photo = GetPhoto(SelectedRemark.Id);
        };

        It should_return_remark = () =>
        {
            Remark.ShouldNotBeNull();
            Remark.Id.ShouldBeEquivalentTo(SelectedRemark.Id);
            Remark.Category.Id.ShouldBeEquivalentTo(SelectedRemark.Category.Id);
            Remark.Author.UserId.ShouldBeEquivalentTo(SelectedRemark.Author.UserId);
            Remark.Description.ShouldBeEquivalentTo(SelectedRemark.Description);
            Remark.Photo.FileId.ShouldBeEquivalentTo(SelectedRemark.Photo.FileId);
        };

        It should_have_photo = () =>
        {
            Photo.ShouldNotBeNull();
            Photo.CanRead.ShouldBeTrue();
        };
    }

    [Subject("Remarks categories")]
    public class when_fetching_remarks_categories : RemarksModule_specs
    {
        static IEnumerable<RemarkCategoryDto> Categories;

        Establish context = () => Initialize(authenticate: false);

        Because of = () => Categories = GetCategories();

        It should_return_non_empty_collection = () =>
        {
            Categories.ShouldNotBeEmpty();
            foreach (var category in Categories)
            {
                category.Id.ShouldNotEqual(Guid.Empty);
                category.Name.ShouldNotBeEmpty();
            }
        };
    }

    [Subject("Remarks create")]
    public class when_creating_remark : RemarksModule_specs
    {
        protected static HttpResponseMessage Result;

        Establish context = () => Initialize(true);

        Because of = () => Result = CreateRemark();

        It should_return_success_status_code = () =>
        {
            Result.IsSuccessStatusCode.ShouldBeTrue();
        };
    }

    [Subject("Remarks delete")]
    public class when_deleting_remark : RemarksModule_specs
    {
        protected static HttpResponseMessage Result;
        static RemarkDto SelectedRemark;
        static IEnumerable<RemarkDto> Remarks;

        Establish context = () =>
        {
            Initialize(true);
            CreateRemark();
            Wait();
            Remarks = GetLatestRemarks();
            SelectedRemark = Remarks.First();
        };

        Because of = () => Result = DeleteRemark(SelectedRemark.Id);

        It should_return_success_status_code = () =>
        {
            Result.IsSuccessStatusCode.ShouldBeTrue();
        };
    }

    [Subject("Remarks resolve")]
    public class when_resolving_remark : RemarksModule_specs
    {
        protected static HttpResponseMessage Result;
        static RemarkDto SelectedRemark;
        static IEnumerable<RemarkDto> Remarks;

        Establish context = () =>
        {
            Initialize(true);
            CreateRemark();
            Wait();
            Remarks = GetLatestRemarks();
            SelectedRemark = Remarks.First(x => x.Resolved == false);
        };

        Because of = () => Result = ResolveRemark(SelectedRemark.Id);

        It should_return_success_status_code = () =>
        {
            Result.IsSuccessStatusCode.ShouldBeTrue();
        };

        It should_be_resolved = () =>
        {
            Wait();
            var remark = GetRemark(SelectedRemark.Id);
            remark.Resolved.ShouldBeTrue();
        };
    }

    [Subject("Remarks resolve")]
    public class when_resolving_remark_from_a_long_distance : RemarksModule_specs
    {
        protected static HttpResponseMessage Result;
        static RemarkDto SelectedRemark;
        static IEnumerable<RemarkDto> Remarks;

        Establish context = () =>
        {
            Initialize(true);
            CreateRemark();
            Wait();
            Remarks = GetLatestRemarks();
            SelectedRemark = Remarks.First(x => x.Resolved == false);
        };

        Because of = () => Result = ResolveRemark(SelectedRemark.Id, 80.0, 80.0);

        It should_return_success_status_code = () =>
        {
            Result.IsSuccessStatusCode.ShouldBeTrue();
        };

        It should_not_be_resolved = () =>
        {
            Wait();
            var remark = GetRemark(SelectedRemark.Id);
            remark.Resolved.ShouldBeFalse();
        };
    }
}