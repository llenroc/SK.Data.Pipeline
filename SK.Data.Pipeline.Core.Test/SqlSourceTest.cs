using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SK.Data.Pipeline.Core.Source;

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
                                        Server = "SKWWT-COMPUTER",
                                        IsTrust = true
                                    })
                                 .AddMonitor((entity) =>
                                 {
                                     count++;
                                     Assert.IsFalse(entity.IsEmpty());
                                 })
                                 .Start();

            Assert.IsTrue(count == 71);
        }
    }
}
