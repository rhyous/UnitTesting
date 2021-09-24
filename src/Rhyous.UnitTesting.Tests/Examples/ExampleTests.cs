using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests
{
    [TestClass]
    public class ArrayNullOrEmptyAttributeExampleTests
    {
        [TestMethod]
        [ArrayNullOrEmpty(typeof(string))]
        public void ArrayNullOrEmptyAttribute_Example_Test(string[] array)
        {
            Assert.IsTrue(array == null || !array.Any());
        }

        [TestMethod]
        [ListTNullOrEmpty(typeof(string))]
        public void ListTNullOrEmptyAttribute_Example_Test(List<string> list)
        {
            Assert.IsTrue(list == null || !list.Any());
        }

        [TestMethod]
        [PrimitiveList(0, -1, int.MinValue)]
        public void PrimitiveList_Example_LessThan1_Test(int i)
        {
            Assert.IsTrue(i < 1);
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void ListTNullOrEmptyAttribute_Example_Test(string str)
        {
            Assert.IsTrue(string.IsNullOrWhiteSpace(str));
        }

        /// <summary>
        /// This test has a few steps and is a bit more complex.
        /// 1. Create your own easy data model. See <see cref="ExampleDataModel"/>.
        /// 2. Make your data model inherit ITestName
        /// 3. Create a file (I put in a Data folder). It should be a relative path.
        /// 4. Right-click on the file in Solution Explorer and go to Properties and set the file to Copy if Newer.
        /// </summary>
        /// <param name="model">The model</param>
        [TestMethod]
        [JsonTestDataSource(typeof(List<ExampleDataModel>), @"Examples\Data\ExampleData.json")] // Notice it is a list here
        public void JsonTestDataSourceAttribute_Example_Test(ExampleDataModel model) // Notice it is a single object here
        {
            var testValue = model.SomeTestValue;
            Assert.IsNotNull(model);
        }
    }
}