using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [TestMethod]
        public void TestTableEntity()
        {
            Entity entity = new Entity();
            entity.SetValue("test", "dfadsf");
            entity.SetValue("col1", 1);

            TableEntity tentity = new TableEntity(entity, "adfa##test####col1##", "addd##test####col1##");
            var afterentity = tentity.ToEntity();
            Assert.IsTrue(entity.EqualsOtherEntity(afterentity));
            Assert.AreEqual(tentity.PartitionKey, "adfadfadsf1");
        }
    }
}
