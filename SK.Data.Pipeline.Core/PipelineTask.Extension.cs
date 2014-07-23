using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public static PipelineTask FromFile(string path)
        {
            return PipelineTask.Create(new FileSourceNode(path));
        }

        public static PipelineTask FromTextFile(string path, string separator = Entity.DefaultSeparator, string[] spiltColumns = null)
        {
            return PipelineTask.Create(new SingleLineFileSourceNode(path))
                               .Spilt(Entity.DefaultColumn, separator, spiltColumns);
        }

        public static PipelineTask FromWeb(string url, CookieContainer cookieContainer = null, ICredentials credential = null)
        {
            return PipelineTask.Create(new WebSourceNode(url, cookieContainer, credential));
        }

        public static PipelineTask FromCsvFile(string path)
        {
            return PipelineTask.Create(new SingleLineFileSourceNode(path))
                               .ParseCsv(Entity.DefaultColumn);
        }

        public static PipelineTask FromXmlFile(string path, XMLEntityModel model)
        {
            return PipelineTask.Create(new TextFileSourceNode(path))
                               .ParseXml(Entity.DefaultColumn, model);
        }

        public static PipelineTask FromJsonWeb(string url, XMLEntityModel model, CookieContainer container = null)
        {
            return PipelineTask.Create(new WebSourceNode(url, container))
                               .ParseJson(Entity.DefaultColumn, model);
        }

        public static PipelineTask FromJsonFile(string path, XMLEntityModel model)
        {
            return PipelineTask.Create(new TextFileSourceNode(path))
                               .ParseJson(Entity.DefaultColumn, model);
        }

        public static PipelineTask FromHtml(string url, EntityModel model, CookieContainer cookieContainer = null, ICredentials credential = null)
        {
            return PipelineTask.Create(new WebSourceNode(url, cookieContainer, credential));
        }

        public static PipelineTask FromSql(string query, ConnectInfo connectInfo, params SqlParameter[] parameters)
        {
            return PipelineTask.Create(new SqlSourceNode(query, connectInfo, parameters));
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
            return AddProcessNode((node) => new RemoveColumnsProcessNode(node, columnsShouldRemove));
        }

        public PipelineTask Filter(Predicate<Entity> shouldFilter)
        {
            return AddProcessNode((node) => new FilterProcessNode(node, shouldFilter));
        }

        public PipelineTask Extend(Action<Entity> extendAction)
        {
            return AddProcessNode((node) => new ExtendProcessNode(node, extendAction));
        }

        public PipelineTask Convert(Func<Entity, Entity> convertFunc)
        {
            return AddProcessNode((node) => new ConvertProcessNode(node, convertFunc));
        }

        public PipelineTask Spilt(string targetColumn, string separator = Entity.DefaultSeparator, string[] spiltColumns = null)
        {
            return AddProcessNode((node) => new SpiltProcessNode(node, targetColumn, separator, spiltColumns));
        }

        public PipelineTask ParseCsv(string targetColumn)
        {
            return AddProcessNode((node) => new ParseCsvProcessNode(node, targetColumn));
        }

        public PipelineTask ParseXml(string targetColumn, XMLEntityModel model)
        {
            return AddProcessNode((node) => new ParseXMLProcessNode(node, targetColumn, model));
        }

        public PipelineTask ParseJson(string targetColumn, XMLEntityModel model)
        {
            return AddProcessNode((node) => new ParseJsonProcessNode(node, targetColumn, model));
        }

        public PipelineTask ParseHtml(string column, XMLEntityModel model)
        {
            return AddProcessNode((node) => new ParseHtmlProcessNode(node, column, model));
        }

        public PipelineTask ParseHtml(XMLEntityModel model)
        {
            return ParseHtml(Entity.DefaultColumn, model);
        }

        public PipelineTask AddTemplateColumn(string targetColumn, string template)
        {
            return AddProcessNode((node) => new AddTemplateColumnProcessNode(_LastNode, targetColumn, template));
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

        public PipelineTask ToFile(string path)
        {
            return To(new FileConsumer(path));
        }

        public PipelineTask ToTextFile(string path, string separator = Entity.DefaultSeparator, string[] columns = null)
        {
            To(new TextFileConsumer(path, separator, columns));
            return this;
        }

        public PipelineTask ToTextFile(string path, EntityModel model, string separator = Entity.DefaultSeparator)
        {
            To(new TextFileConsumer(path, model, separator));
            return this;
        }

        public PipelineTask ToCsvFile(string path, EntityModel model)
        {
            return ToTextFile(path, model);
        }

        public PipelineTask ToCsvFile(string path, string[] columns = null)
        {
            return ToTextFile(path, ", ", columns);
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
