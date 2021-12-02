namespace Rhyous.UnitTesting
{
    /// <summary>An interface that allows for naming the test.</summary>
    public interface ITestRunOrder
    {
        /// <summary>The order in which the tests will run</summary>
        int RunOrder { get; set; }
    }
}
