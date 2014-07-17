using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SK.Data.Pipeline.Core.Test
{
    [TestClass]
    public class ProcessTest
    {
        const string SimpleSource = "SimpleSource";
        const string Output = "Output";

        [TestMethod]
        public void Filter()
        {
            int beforeFilter = 0, afterFilter = 0;
            PipelineTask.Create(new SingleLineFileSourceNode(SimpleSource))
                        .Spilt(Entity.DefaultColumn)
                        .AddMonitor((sender, args) =>
                        {
                            beforeFilter++;
                        })
                        .Filter((entity) =>
                        {
                            return true;
                        })
                        .AddMonitor((sender, args) =>
                        {
                            afterFilter++;
                        })
                        .Start();

            Assert.AreEqual(1, beforeFilter);
            Assert.AreEqual(0, afterFilter);
        }

        [TestMethod]
        public void Extend()
        {
            PipelineTask.Create(new SingleLineFileSourceNode(SimpleSource))
                        .Spilt(Entity.DefaultColumn)
                        .AddMonitor((sender, args) =>
                        {
                            Assert.AreEqual(2, args.CurrentEntity.Values.Keys.Count);
                        })
                        .Extend((entity) =>
                        {
                            entity.SetValue("a", "");
                        })
                        .AddMonitor((sender, args) =>
                        {
                            Assert.AreEqual(3, args.CurrentEntity.Values.Keys.Count);
                        })
                        .Start();
        }

        [TestMethod]
        public void Convert()
        {
            PipelineTask.Create(new SingleLineFileSourceNode(SimpleSource))
                        .Spilt(Entity.DefaultColumn)
                        .AddMonitor((sender, args) =>
                        {
                            Assert.AreEqual(2, args.CurrentEntity.Values.Keys.Count);
                        })
                        .Convert((entity) =>
                        {
                            return new Entity();
                        })
                        .AddMonitor((sender, args) =>
                        {
                            Assert.AreEqual(0, args.CurrentEntity.Values.Keys.Count);
                        })
                        .Start();
        }
    }
}
