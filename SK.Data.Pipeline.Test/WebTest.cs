using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SK.Data.Pipeline.Core;
using SK.Data.Pipeline.Web;

namespace SK.Data.Pipeline.Test
{
    [TestClass]
    public class WebTest
    {
        const string Province = "Province";
        const string Output = "Output";

        [TestMethod]
        public void HtmlBasic()
        {
            XMLEntityModel model = new XMLEntityModel(@"//table[@class='wikitable sortable']/tr[not(@*)]");
            model.AddXMLColumn("GB", "./td[1]");
            model.AddXMLColumn("Province", "./td[3]");

            PipelineTask.FromWeb("http://en.wikipedia.org/wiki/China_provinces")
                        .ParseHtml(model)
                        .ToFile(Output, model)
                        .Start();

            TestHelper.CompareTwoFile(Province, Output);
        }
    }
}
