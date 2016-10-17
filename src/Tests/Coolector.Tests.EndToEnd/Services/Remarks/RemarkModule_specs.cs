using System;
using System.Collections.Generic;
using System.IO;
using Coolector.Services.Remarks.Domain;
using Coolector.Tests.EndToEnd.Framework;
using Machine.Specifications;
using System.Linq;

namespace Coolector.Tests.EndToEnd.Services.Remarks
{
    public abstract class RemarkModule_specs
    {
        protected static IHttpClient HttpClient = new CustomHttpClient("http://localhost:10002");
        protected static Remark Remark;
        protected static IEnumerable<Remark> Remarks;
        protected static IEnumerable<Category> Categories;
        protected static Guid RemarkId;

        protected static void InitializeAndFetch()
        {
            var remark = FetchRemarks().First();
            RemarkId = remark.Id;
        }

        protected static IEnumerable<Remark> FetchRemarks()
            => HttpClient.GetAsync<IEnumerable<Remark>>("remarks?latest=true").WaitForResult();

        protected static IEnumerable<Category> FetchCategories()
            => HttpClient.GetAsync<IEnumerable<Category>>("remarks/categories").WaitForResult();

        protected static Remark FetchRemark(Guid id)
            => HttpClient.GetAsync<Remark>($"remarks/{id}").WaitForResult();

        protected static Stream FetchPhoto(Guid id)
            => HttpClient.GetStreamAsync($"remarks/{id}/photo").WaitForResult();
    }

    [Subject("RemarkService fetch remarks")]
    public class when_fetching_remarks : RemarkModule_specs
    {
        Because of = () => Remarks = FetchRemarks();

        It should_not_be_null = () => Remarks.ShouldNotBeNull();

        It should_not_be_empty = () => Remarks.ShouldNotBeEmpty();
    }

    [Subject("RemarkService fetch single remark")]
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

    [Subject("RemarkService fetch categories")]
    public class when_fetching_categories : RemarkModule_specs
    {
        Because of = () => Categories = FetchCategories();

        It should_not_be_null = () => Categories.ShouldNotBeNull();

        It should_not_be_empty = () => Categories.ShouldNotBeEmpty();
    }

    [Subject("RemarkService fetch photo")]
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