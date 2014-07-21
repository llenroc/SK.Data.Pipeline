using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public static PipelineTask FromWeb(string url, ICredentials credential = null)
        {
            return PipelineTask.Create(new WebSourceNode(url, credential));
        }

        public static PipelineTask FromCsvFile(string path)
        {
            return PipelineTask.Create(new SingleLineFileSourceNode(path))
                               .ParseCsv(Entity.DefaultColumn);
        }

        public static PipelineTask FromXmlFile(string path, XMLEntityModel model)
        {
            return PipelineTask.Create(new FileSourceNode(path))
                               .ParseXml(Entity.DefaultColumn, model)
                               .RemoveFields(Entity.DefaultColumn);
        }

        public static PipelineTask FromHtml(string url, EntityModel model, ICredentials credential = null)
        {
            return PipelineTask.Create(new WebSourceNode(url, credential));
                               
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
            AddProcessNode((node) => new RemoveColumnsProcessNode(node, columnsShouldRemove));

            return this;
        }

        public PipelineTask Filter(Predicate<Entity> shouldFilter)
        {
            AddProcessNode((node) => new FilterProcessNode(node, shouldFilter));

            return this;
        }

        public PipelineTask Extend(Action<Entity> extendAction)
        {
            AddProcessNode((node) => new ExtendProcessNode(node, extendAction));

            return this;
        }

        public PipelineTask Convert(Func<Entity, Entity> convertFunc)
        {
            AddProcessNode((node) => new ConvertProcessNode(node, convertFunc));

            return this;
        }

        public PipelineTask Spilt(string targetColumn, string separator = Entity.DefaultSeparator, string[] spiltColumns = null)
        {
            AddProcessNode((node) => new SpiltProcessNode(node, targetColumn, separator, spiltColumns));

            return this;
        }

        public PipelineTask ParseCsv(string targetColumn)
        {
            AddProcessNode((node) => new ParseCsvProcessNode(node, targetColumn));

            return this;
        }

        public PipelineTask ParseXml(string targetColumn, XMLEntityModel model)
        {
            AddProcessNode((node) => new ParseXMLProcessNode(node, targetColumn, model));

            return this;
        }
        public PipelineTask ParseHtml(string column, XMLEntityModel model)
        {
            AddProcessNode((node) => new ParseHtmlProcessNode(node, column, model));
            return this;
        }

        public PipelineTask ParseHtml(XMLEntityModel model)
        {
            return ParseHtml(Entity.DefaultColumn, model);
        }

        public PipelineTask AddTemplateColumn(string targetColumn, string template)
        {
            AddProcessNode((node) => new AddTemplateColumnProcessNode(_LastNode, targetColumn, template));

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

        public PipelineTask ToFile(string path, EntityModel model, string separator = Entity.DefaultSeparator)
        {
            To(new FileConsumer(path, model, separator));
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

        public PipelineTask AddMonitor(Action<Entity> monitorAction)
        {
            To(new MonitorConsumer(monitorAction));

            return this;
        }
        #endregion
    }
}
