using Machine.Specifications;

namespace Coolector.Tests
{
    [Subject("Test")]
    public class When_dummy_test
    {
        protected bool Value;

        Because of => () => Value = true;

        It should_be_true => () => Value.ShouldBeTrue();
    }
}
