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
            PipelineTask.Create(new SingleLineFileSourceNode(SimpleSource))
                    .Spilt(Entity.DefaultColumn)
                    .To(new FileConsumer(SimpleFileOutput))
                    .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SimpleSource, SimpleFileOutput));
        }

        [TestMethod]
        public void SpiltByT()
        {
            PipelineTask.Create(new SingleLineFileSourceNode(SimpleSourceT))
                    .Spilt(Entity.DefaultColumn, separator: "\t")
                    .To(new FileConsumer(SimpleFileOutput))
                    .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SimpleSource, SimpleFileOutput));
        }

        [TestMethod]
        public void AddTemplateColumn()
        {
            PipelineTask.Create(new SingleLineFileSourceNode(SimpleSourceT))
                    .Spilt(Entity.DefaultColumn, separator: "\t")
                    .AddTemplateColumn("Template", "##col1## ##col2")
                    .ToFile(TemplateFileOutput)
                    .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SampleTemplateFileOutput, TemplateFileOutput));
        }

        [TestMethod]
        public void FromFileToTemplateFile()
        {
            PipelineTask.Create(new SingleLineFileSourceNode(SimpleSourceT))
                    .Spilt(Entity.DefaultColumn, separator: "\t")
                    .ToTemplateFile(TemplateFileOutput, "##col1## dddd ##col2##")
                    .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SimpleSource, SimpleFileOutput));
        }

        [TestMethod]
        public void MonitorConsumer()
        {
            int count = 0;
            PipelineTask.Create(new SingleLineFileSourceNode(SimpleSource))
                    .AddMonitor((sender, args) =>
                                {
                                    count++;
                                })
                    .Start();

            Assert.AreEqual(2, count);
        }
    }
}
