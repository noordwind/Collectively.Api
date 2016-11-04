using Coolector.Api.Tests.EndToEnd.Framework;
using Coolector.Dto.Remarks;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using System.Linq;

namespace Coolector.Api.Tests.EndToEnd.Modules
{
    public abstract class RemarksModule_specs : ModuleBase_specs
    {
        protected static IPhotoGenerator PhotoGenerator = new PhotoGenerator(); 

        protected static RemarkDto GetRemark(Guid id)
            => HttpClient.GetAsync<RemarkDto>($"remarks/{id}").WaitForResult();

        protected static IEnumerable<RemarkDto> GetLatestRemarks()
            => HttpClient.GetCollectionAsync<RemarkDto>("remarks?latest=true").WaitForResult();

        protected static IEnumerable<RemarkDto> GetNearestRemarks()
            => HttpClient.GetCollectionAsync<RemarkDto>("remarks?radius=10000&longitude=1.0&latitude=1.0").WaitForResult();

        protected static IEnumerable<RemarkDto> GetRemarksWithCategory(string categoryName)
            => HttpClient.GetCollectionAsync<RemarkDto>($"remarks?radius=10000&longitude=1.0&latitude=1.0&categories={categoryName}").WaitForResult();

        protected static IEnumerable<RemarkDto> GetRemarksWithState(string state)
            => HttpClient.GetCollectionAsync<RemarkDto>($"remarks?radius=10000&longitude=1.0&latitude=1.0&state={state}").WaitForResult();

        protected static IEnumerable<RemarkCategoryDto> GetCategories()
            => HttpClient.GetCollectionAsync<RemarkCategoryDto>("remarks/categories").WaitForResult();

        protected static HttpResponseMessage CreateRemark(double latitude = 1.0, double longitude = 1.0, string categoryName = null)
        {
            var categories = GetCategories().ToList();
            var photo = GeneratePhoto();
            var category = categories.FirstOrDefault(x => x.Name == categoryName) ?? categories.First();

            return HttpClient.PostAsync("remarks", new
            {
                Address = "",
                Category = category.Name,
                Description = "test",
                Latitude = latitude,
                Longitude = longitude,
                Photo = photo
            }).WaitForResult();
        }

        protected static HttpResponseMessage DeleteRemark(Guid remarkId)
            => HttpClient.DeleteAsync($"remarks/{remarkId}").WaitForResult();

        protected static HttpResponseMessage ResolveRemark(Guid remarkId, 
            double latitude = 1.0, double longitude = 1.0,
            bool validatePhoto = false, bool validateLocation = false)
            => HttpClient.PutAsync("remarks", new
            {
                RemarkId = remarkId,
                Photo = GeneratePhoto(),
                Latitude = latitude,
                Longitude = longitude,
                ValidatePhoto = validatePhoto,
                ValidateLocation = validateLocation
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
            }
        };
    }

    [Subject("Remarks collection")]
    public class when_fetching_nearest_remarks : RemarksModule_specs
    {
        protected static IEnumerable<RemarkDto> Remarks;

        Establish context = () =>
        {
            Initialize(true);
            CreateRemark();
            CreateRemark(1.1, 1.1);
            CreateRemark(1.3, 1.3);
            Wait();
        };

        Because of = () => Remarks = GetNearestRemarks().ToList();

        It should_return_non_empty_collection = () =>
        {
            Remarks.ShouldNotBeEmpty();
            var i = 0;
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
            }
        };

        It should_return_nearest_remark_first = () =>
        {
            var remark = Remarks.FirstOrDefault();
            remark.Location.Coordinates[0].ShouldEqual(1.0);
            remark.Location.Coordinates[1].ShouldEqual(1.0);
        };

        It should_return_remarks_in_correct_order = () =>
        {
            RemarkDto previousRemark = null;
            foreach (var remark in Remarks)
            {
                if (previousRemark != null)
                {
                    previousRemark.Location.Coordinates[0].ShouldBeLessThanOrEqualTo(remark.Location.Coordinates[0]);
                    previousRemark.Location.Coordinates[1].ShouldBeLessThanOrEqualTo(remark.Location.Coordinates[1]);
                }
                previousRemark = remark;
            }
        };
    }

    [Subject("Remarks collection")]
    public class when_fetching_remarks_with_specific_category : RemarksModule_specs
    {
        protected static string Category = "damages";
        protected static IEnumerable<RemarkDto> Remarks;

        Establish context = () =>
        {
            Initialize(true);
            CreateRemark(categoryName: Category);
            Wait();
        };

        Because of = () => Remarks = GetRemarksWithCategory(Category);

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
            }
        };

        It should_contain_remarks_with_the_same_category = () =>
        {
            Remarks.All(x => x.Category.Name == Category).ShouldBeTrue();
        };
    }

    [Subject("Remarks collection")]
    public class when_fetching_resolved_remarks : RemarksModule_specs
    {
        protected static string State = "resolved";
        protected static IEnumerable<RemarkDto> Remarks;

        Establish context = () =>
        {
            Initialize(true);
        };

        Because of = () => Remarks = GetRemarksWithState(State);

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
            }
        };

        It should_contain_only_resolved_remarks = () =>
        {
            Remarks.All(x => x.Resolved).ShouldBeTrue();
        };
    }

    [Subject("Remarks collection")]
    public class when_fetching_active_remarks : RemarksModule_specs
    {
        protected static string State = "active";
        protected static IEnumerable<RemarkDto> Remarks;

        Establish context = () =>
        {
            Initialize(true);
        };

        Because of = () => Remarks = GetRemarksWithState(State);

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
            }
        };

        It should_contain_only_active_remarks = () =>
        {
            Remarks.All(x => x.Resolved == false).ShouldBeTrue();
        };
    }

    [Subject("Remark details")]
    public class when_fetching_remark : RemarksModule_specs
    {
        static IEnumerable<RemarkDto> Remarks;
        static RemarkDto SelectedRemark;
        static RemarkDto Remark;

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
        };

        It should_return_remark = () =>
        {
            Remark.ShouldNotBeNull();
            Remark.Id.ShouldBeEquivalentTo(SelectedRemark.Id);
            Remark.Category.Id.ShouldBeEquivalentTo(SelectedRemark.Category.Id);
            Remark.Author.UserId.ShouldBeEquivalentTo(SelectedRemark.Author.UserId);
            Remark.Description.ShouldBeEquivalentTo(SelectedRemark.Description);
        };

        It should_have_photo = () =>
        {
            Remark.Photos.ShouldNotBeEmpty();
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

    [Ignore("depends on api's feature switch")]
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