using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SK.Data.Pipeline.Test
{
    using Pipeline = SK.Data.Pipeline.Core.PipelineTask;
    using SK.Data.Pipeline.Core;
    using SK.Data.Pipeline.RazorTemplate;

    [TestClass]
    public class RazorTest
    {
        const string SimpleSource = "SimpleSource";
        const string RazorTemplateColumnFile = "RazorTemplateColumn";
        const string RazorTemplateFile = "RazorTemplateFile";
        const string SampleRazorTemplateSource = "SampleRazorTemplateSource";
        const string SampleRazorTemplateFile = "SampleRazorTemplateFile";
        
        [TestMethod]
        public void ToRazorTemplateColumn()
        {
            Pipeline.Create(new SingleLineFileSourceNode(SimpleSource))
                    .Spilt(Entity.DefaultColumn)
                    .AddRazorTemplateColumn("Template", @"@Model.col1 **** @Model.col2")
                    .ToFile(RazorTemplateColumnFile)
                    .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SampleRazorTemplateSource, RazorTemplateColumnFile));
        }

        [TestMethod]
        public void ToRazorTemplateFile()
        {
            Pipeline.Create(new SingleLineFileSourceNode(SimpleSource))
                    .Spilt(Entity.DefaultColumn)
                    .ToRazorFile(RazorTemplateFile,
@"<tr><td>@Model.col1</td><td>@Model.col2</td></tr>",
@"@for(int i = 0; i < 5; ++i){ <b> @i </b>}
<table>",
@"</table>")
                    .Start();

            Assert.IsTrue(TestHelper.CompareTwoFile(SampleRazorTemplateFile, RazorTemplateFile));
        }
    }
}
