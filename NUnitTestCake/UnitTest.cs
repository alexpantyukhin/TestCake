namespace NUnitTestCake;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        // Act
        var connString = TestCake.TestCake.GetMyConfiguration("cs2");

        Console.WriteLine($"CONNECTION STRING: {connString}");

        // Assert
        Assert.IsNotNull(connString);
    }
}