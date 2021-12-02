namespace Rhyous.UnitTesting
{
    /// <summary>A standard model that works with could meet most TestDataSource need.</summary>
    /// <typeparam name="T">The type of both the TestValue and the expected value.</typeparam>
    /// <remarks>Use this when the TestValue and the Expected value are the same type.</remarks>
    public class TestDataRow<T> : TestDataRow<T, T> { }

    /// <summary>A standard model that works with could meet most TestDataSource need.</summary>
    /// <typeparam name="TValue">The type of the TestValue.</typeparam>
    /// <typeparam name="TExpected">The type of the expected value.</typeparam>
    /// <remarks>Use this when the TestValue and the Expected value are different types.</remarks>
    public class TestDataRow<TValue, TExpected> : ITestName, ITestRunOrder
    {
        /// <summary>The order that this test will run in. If this is not provided, the order could be random.</summary>
        public int RunOrder { get; set; }
        /// <summary>The name of the test.</summary>
        public string TestName { get; set; }
        /// <summary>The description of the test.</summary>
        public string Description { get; set; }
        /// <summary>The value to test of the test. If you want more than one value, just make this an array. It could match an Expected array.</summary>
        public TValue TestValue { get; set; }
        /// <summary>The expected value of the test. If you want more than one value, just make this an array. It could match a TestValue array.</summary>
        public TExpected Expected { get; set; }
        /// <summary>The message to pass into any Assert methods.</summary>
        public string Message { get; set; }
    }
}