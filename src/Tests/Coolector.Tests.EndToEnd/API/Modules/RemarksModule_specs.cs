using System.Collections.Generic;
using Coolector.Dto.Remarks;
using Machine.Specifications;

namespace Coolector.Tests.EndToEnd.API.Modules
{
    public abstract class RemarksModule_specs : ModuleBase_specs
    {
        protected static void Initialize()
        {
        }
    }

    [Subject(("Remarks"))]
    public class when_fetching_the_latest_remarks : RemarksModule_specs
    {
        static IEnumerable<RemarkDto> Remarks;

        Establish context = () =>
        {
            Initialize();
        };

        Because of = () => Remarks = HttpClient.GetCollectionAsync<RemarkDto>("remarks").GetAwaiter().GetResult();

        It should_return_non_empty_collection = () =>
        {
            Remarks.ShouldNotBeEmpty();
        };
    }
}