using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rhyous.UnitTesting.Tests
{
    [XmlRoot("Row")]
    [XmlType("Row")]
    public class TestRow
    {
        public string TestName { get; set; }
        public string Value { get; set; }
        public string ExpectedResult { get; set; }
        public string Message { get; set; }
    }

    [XmlRoot("Rows")]
    public class TestRows : List<TestRow>
    {
    }
}
