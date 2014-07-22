using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SK.Data.Pipeline.Core.Test
{
    [TestClass]
    public class SqlSourceTest
    {
        [TestMethod]
        public void LoadFromSql()
        {
            int count = 0;
            PipelineTask.FromSql("select * from datasetinfo",
                                    new ConnectInfo()
                                    {
                                        Db = "DatasetInfo",
                                        Server = ".",
                                        IsTrust = true
                                    })
                                 .AddMonitor((entity) =>
                                 {
                                     count++;
                                     Assert.IsFalse(entity.IsEmpty());
                                 })
                                 .ToCsvFile("CSVOutput")
                                 .Start();

            Assert.IsTrue(count == 71);
        }
    }
}
