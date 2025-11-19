using Rhyous.EasyCsv;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.UnitTesting
{
    internal static class CsvExtensions
    {
        public static void GetHeaderColumnIndexes(this ICsv csv, string testNameColumn, List<int> headerColumnIndexes)
        {
            if (csv.Headers != null && csv.Headers.Any())
            {
                if (!string.IsNullOrWhiteSpace(testNameColumn))
                {
                    if (testNameColumn.Contains(","))
                    {
                        var values = testNameColumn.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(h => csv.Headers.IndexOf(h));
                        if (values != null && values.Any())
                            headerColumnIndexes.AddRange(values);
                    }
                    else
                    {
                        var columnIndex = csv.Headers.IndexOf(testNameColumn);
                        if (columnIndex > -1)
                            headerColumnIndexes.Add(columnIndex);
                        else
                        {
                            columnIndex = csv.Headers.IndexOf(nameof(ITestName.TestName));
                            if (columnIndex > -1)
                                headerColumnIndexes.Add(columnIndex);
                        }
                    }
                }
            }
        }
    }
}
