using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SK.Data.Pipeline.Core.Test
{
    [TestClass]
    public class BasicTest
    {
        const string SimpleSource = "SimpleSource";
        const string SimpleSourceT = "SimpleSourceT";
        const string SimpleFileOutput = "SimpleFileOutput";
        const string TemplateFileOutput = "TemplateFileOutput";
        const string SampleTemplateFileOutput = "SampleTemplateFileOutput";

        [TestMethod]
        public void FromFileToFile()
        {
            Pipeline.Create(new FileSourceNode(SimpleSource))
                    .SpiltParse(Entity.DefaultColumn)
                    .To(new FileConsumer(SimpleFileOutput))
                    .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SimpleSource, SimpleFileOutput));
        }

        [TestMethod]
        public void SpiltBy_T()
        {
            Pipeline.Create(new FileSourceNode(SimpleSourceT))
                    .SpiltParse(Entity.DefaultColumn, separator: "\t")
                    .To(new FileConsumer(SimpleFileOutput))
                    .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SimpleSource, SimpleFileOutput));
        }

        [TestMethod]
        public void AddTemplateColumn()
        {
            Pipeline.Create(new FileSourceNode(SimpleSourceT))
                    .SpiltParse(Entity.DefaultColumn, separator: "\t")
                    .AddTemplateColumn("##col1## ##col2", "Template")
                    .ToFile(TemplateFileOutput)
                    .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SampleTemplateFileOutput, TemplateFileOutput));
        }

        [TestMethod]
        public void FromFileToTemplateFile()
        {
            Pipeline.Create(new FileSourceNode(SimpleSourceT))
                    .SpiltParse(Entity.DefaultColumn, separator: "\t")
                    .ToTemplateFile(TemplateFileOutput, "##col1## dddd ##col2##")
                    .Start();

            //Assert.IsTrue(TestHelper.CompareTwoFile(SimpleSource, SimpleFileOutput));
        }
    }
}
