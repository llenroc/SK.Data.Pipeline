using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SK.Data.Pipeline.Core.Test
{
    [TestClass]
    public class CsvXMLJsonTest
    {
        const string SampleSource = "SampleSource";
        const string XmlSource = "Source.xml";
        const string Output = "Output";

        [TestMethod]
        public void CsvBasic()
        {
            PipelineTask.FromCsvFile(SampleSource)
                        .ToFile(Output)
                        .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SampleSource, Output));
        }

        [TestMethod]
        public void XmlBasic()
        {
            PipelineTask.FromXmlFile(XmlSource, @".//Entity", "./col1", "./col2")
                        .ToFile(Output)
                        .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SampleSource, Output));
        }
    }
}
