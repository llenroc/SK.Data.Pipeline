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

        [TestMethod]
        public void FromFileToFile()
        {
            Pipeline.Create(new FileSourceNode(SimpleSource))
                    .SpiltParse()
                    .To(new FileConsumer(SimpleFileOutput))
                    .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SimpleSource, SimpleFileOutput));
        }

        [TestMethod]
        public void FromFileToFile_T()
        {
            Pipeline.Create(new FileSourceNode(SimpleSourceT))
                    .SpiltParse(separator: "\t")
                    .To(new FileConsumer(SimpleFileOutput))
                    .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SimpleSource, SimpleFileOutput));
        }
    }
}
