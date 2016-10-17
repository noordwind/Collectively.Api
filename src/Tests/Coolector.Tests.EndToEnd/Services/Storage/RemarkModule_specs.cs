using System;
using System.Collections.Generic;
using System.IO;
using Coolector.Tests.EndToEnd.Framework;
using Machine.Specifications;
using System.Linq;
using Coolector.Dto.Remarks;

namespace Coolector.Tests.EndToEnd.Services.Storage
{
    public abstract class RemarkModule_specs
    {
        protected static IHttpClient HttpClient = new CustomHttpClient("http://localhost:10000");
        protected static RemarkDto Remark;
        protected static IEnumerable<RemarkDto> Remarks;
        protected static IEnumerable<RemarkCategoryDto> Categories;
        protected static Guid RemarkId;

        protected static void InitializeAndFetch()
        {
            var remark = FetchRemarks().First();
            RemarkId = remark.Id;
        }

        protected static IEnumerable<RemarkDto> FetchRemarks()
            => HttpClient.GetAsync<IEnumerable<RemarkDto>>("remarks?latest=true").WaitForResult();

        protected static IEnumerable<RemarkCategoryDto> FetchCategories()
            => HttpClient.GetAsync<IEnumerable<RemarkCategoryDto>>("remarks/categories").WaitForResult();

        protected static RemarkDto FetchRemark(Guid id)
            => HttpClient.GetAsync<RemarkDto>($"remarks/{id}").WaitForResult();

        protected static Stream FetchPhoto(Guid id)
            => HttpClient.GetStreamAsync($"remarks/{id}/photo").WaitForResult();
    }

    [Subject("StorageService fetch remarks")]
    public class when_fetching_remarks : RemarkModule_specs
    {
        Because of = () => Remarks = FetchRemarks();

        It should_not_be_null = () => Remarks.ShouldNotBeNull();

        It should_not_be_empty = () => Remarks.ShouldNotBeEmpty();
    }

    [Subject("StorageService fetch single remark")]
    public class when_fetching_single_remark : RemarkModule_specs
    {
        Establish context = () => InitializeAndFetch();

        Because of = () => Remark = FetchRemark(RemarkId);

        It should_not_be_null = () => Remark.ShouldNotBeNull();

        It should_have_correct_id = () => Remark.Id.ShouldEqual(RemarkId);

        It should_return_remark = () =>
        {
            Remark.Id.ShouldNotEqual(Guid.Empty);
            Remark.Author.ShouldNotBeNull();
            Remark.Category.ShouldNotBeNull();
            Remark.CreatedAt.ShouldNotEqual(default(DateTime));
            Remark.Description.ShouldNotBeEmpty();
            Remark.Location.ShouldNotBeNull();
            Remark.Photo.ShouldNotBeNull();
        };
    }

    [Subject("StorageService fetch categories")]
    public class when_fetching_categories : RemarkModule_specs
    {
        Because of = () => Categories = FetchCategories();

        It should_not_be_null = () => Categories.ShouldNotBeNull();

        It should_not_be_empty = () => Categories.ShouldNotBeEmpty();
    }

    [Subject("StorageService fetch photo")]
    public class when_fetching_photo : RemarkModule_specs
    {
        protected static Stream Photo;

        Establish context = () => InitializeAndFetch();

        Because of = () => Photo = FetchPhoto(RemarkId);

        It should_not_be_null = () =>
        {
            Photo.ShouldNotBeNull();
            Photo.CanRead.ShouldBeTrue();
        };
    }
}