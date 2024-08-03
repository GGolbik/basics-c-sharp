using Xunit;
using FluentAssertions;

namespace ggolbik.csharp
{
  public class ExampleTest
  {
    [Fact]
    public void testIncrease()
    {
      int value = 5;
      Example example = new Example();
      int actual = example.increase(value);
      actual.Should().Be(value + 1, "Erhöhung um 1 hat nicht geklappt.");
    }

    [Fact]
    public void testIncreaseFail()
    {
      int value = 5;
      Example example = new Example();
      int actual = example.increase(value);
      actual.Should().Be(2, "A description of the expectation.");
    }
  }
}
