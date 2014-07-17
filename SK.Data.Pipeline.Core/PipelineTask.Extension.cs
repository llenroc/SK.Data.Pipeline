using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public partial class PipelineTask
    {
        #region Create Functions
        public static PipelineTask Create(SourceNode source)
        {
            return new PipelineTask(source);
        }

        public static PipelineTask Create(Func<IEnumerable<Entity>> getEntitiesFunc)
        {
            return Create(new FuncSourceNode(getEntitiesFunc));
        }

        public static PipelineTask FromCsvFile(string path)
        {
            return PipelineTask.Create(new SingleLineFileSourceNode(path))
                               .ParseCsv(Entity.DefaultColumn);
        }

        public static PipelineTask FromXmlFile(string path, string itemXPath, params string[] columnXPaths)
        {
            return PipelineTask.Create(new FileSourceNode(path))
                               .ParseXml(Entity.DefaultColumn, itemXPath, columnXPaths)
                               .RemoveFields(Entity.DefaultColumn);
        }
        #endregion

        #region Add Process Node
        public PipelineTask AddProcessNode(Func<DataNode, ProcessNode> createProcessNode)
        {
            _LastNode = createProcessNode(_LastNode);

            return this;
        }

        public PipelineTask RemoveFields(params string[] columnsShouldRemove)
        {
            AddProcessNode((node) => new RemoveColumns(node, columnsShouldRemove));

            return this;
        }

        public PipelineTask Filter(Predicate<Entity> shouldFilter)
        {
            AddProcessNode((node) => new Filter(node, shouldFilter));

            return this;
        }

        public PipelineTask Extend(Action<Entity> extendAction)
        {
            AddProcessNode((node) => new Extend(node, extendAction));

            return this;
        }

        public PipelineTask Convert(Func<Entity, Entity> convertFunc)
        {
            AddProcessNode((node) => new Convert(node, convertFunc));

            return this;
        }

        public PipelineTask Spilt(string column, string separator = Entity.DefaultSeparator, string[] spiltColumns = null)
        {
            AddProcessNode((node) => new Spilt(node, column, separator, spiltColumns));

            return this;
        }

        public PipelineTask ParseCsv(string column)
        {
            AddProcessNode((node) => new ParseCsv(node, column));

            return this;
        }

        public PipelineTask ParseXml(string column, string itemXPath, params string[] columnXPaths)
        {
            AddProcessNode((node) => new ParseXML(node, column, itemXPath, columnXPaths));

            return this;
        }

        public PipelineTask AddTemplateColumn(string column, string template)
        {
            AddProcessNode((node) => new AddTemplateColumn(_LastNode, column, template));

            return this;
        }
        #endregion

        #region Add Consumer
        public PipelineTask To(ConsumerBase consumer)
        {
            _LastNode.Start += consumer.Start;
            _LastNode.GetFirstEntity += consumer.GetFirstEntity;
            _LastNode.GetEntity += consumer.Consume;
            _LastNode.Finish += consumer.Finish;
            return this;
        }

        public PipelineTask ToFile(string path, string separator = Entity.DefaultSeparator, string[] columns = null)
        {
            To(new FileConsumer(path, separator, columns));
            return this;
        }

        public PipelineTask ToTemplateFile(string path, string template)
        {
            To(new TemplateFileConsumer(path, template));
            return this;
        }

        public PipelineTask AddMonitor(Action<object, GetEntityEventArgs> monitorAction)
        {
            To(new MonitorConsumer(monitorAction));
            
            return this;
        }
        #endregion
    }
}
