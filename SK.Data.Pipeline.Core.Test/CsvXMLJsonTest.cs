using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace SK.Data.Pipeline.Core.Test
{
    [TestClass]
    public class CsvXMLJsonTest
    {
        const string SampleSource = "SampleSource";
        const string XmlSource = "Source.xml";
        const string Output = "Output";
        const string SampleJsonOutput = "SampleJsonOutput";

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
            XMLEntityModel model = new XMLEntityModel(@".//Entity");
            model.AddXMLColumn("col1", "./col1");
            model.AddXMLColumn("col2", "./col2");

            PipelineTask.FromXmlFile(XmlSource, model)
                        .ToFile(Output)
                        .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SampleSource, Output));
        }

        [TestMethod]
        public void JsonBasic()
        {
            XMLEntityModel model = new XMLEntityModel(@".//Results");
            model.AddXMLColumn("Name", "./Name");
            model.AddXMLColumn("Desc", "./Desc");

            PipelineTask.FromJsonFile("Course", model)
                        .ToFile(Output)
                        .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SampleJsonOutput, Output));
        }
    }
}
