using Machine.Specifications;

namespace Coolector.Tests.Unit
{
    [Subject("Dummy")]
    public class DummyTest
    {
        static bool value;

        Establish context = () =>
        {
            value = true;
        };

        Because of = () =>
        {

        };

        It should_be_true = () => value.ShouldBeTrue();
    }
}