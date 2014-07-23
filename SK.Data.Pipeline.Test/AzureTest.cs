using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SK.Data.Pipeline.Azure;
using SK.Data.Pipeline.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Test
{
    [TestClass]
    public class AzureTest
    {
        private AzureTableInfo info = new AzureTableInfo("pipelinetest", "zCOKPdsKFSMrQ6CNe1j945Z1Gl3mMUxfWMO2jPaIYAYqBcuOejx0lsWZbIu1hf7toJmPrRbEFN75UBHsWjb11A==", "TestTable");

        [TestMethod]
        public void AzureTableConsumer()
        {
            PipelineTask.FromCsvFile("SimpleSource")
                    .AddMonitor((entity) =>
                    {
                        Console.WriteLine();
                    })
                    .ToAzureTable(info, "##col1##", "##col1####col2##")
                    .Start();
        }

        [TestMethod]
        public void AzureTableSource()
        {
            int count = 0;
            PipelineTask.Create(new AzureTableSourceNode(info))
                    .AddMonitor((entity) =>
                    {
                        count++;
                    })
                    .Start();

            Assert.AreEqual(2, count);
        }
    }
}
