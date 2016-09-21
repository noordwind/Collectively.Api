using FluentAssertions;
using NUnit.Framework;

namespace Coolector.Tests.Unit
{
    [TestFixture]
    public class DummyTest
    {
        [Test]
        public void TrueShouldBeTrue()
        {
            true.Should().BeTrue();
        }
    }
}