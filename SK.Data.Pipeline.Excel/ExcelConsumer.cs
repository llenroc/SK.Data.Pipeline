
using Microsoft.Office.Interop.Excel;
using SK.Data.Pipeline.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Excel
{
    using Excel = Microsoft.Office.Interop.Excel;
    public class ExcelConsumer : ConsumerBase
    {
        public string Path {set;get;}

        public EntityModel Model { get; set; }

        private object MissValue = System.Reflection.Missing.Value;
        private Excel.Application _Application { get; set; }
        private Excel.Workbook _Workbook { get; set; }
        private Excel.Worksheet _WorkSheet { get; set; }

        /// <param name="path">This path should be Absolute path, or you may can not found the oputput file.</param>
        public ExcelConsumer(string path, string[] columns)
            :this(path, new EntityModel(columns))
        {  }

        /// <param name="path">This path should be Absolute path, or you may can not found the oputput file.</param>
        public ExcelConsumer(string path, EntityModel model = null)
        {
            Path = path;
            Model = model;
        }

        public override void Start(object sender, StartEventArgs args)
        {
            InitExcel();
        }

        public override void GetFirstEntity(object sender, FirstEntityEventArgs args)
        {
            // Initial Model use first entity if Model == null
            if (Model == null)
            {
                Model = new EntityModel(args.FirstEntity.Columns);
            }

            // Write Title use model columns
            for (int i = 0; i < Model.Columns.Count(); ++i)
            {
                _WorkSheet.Cells[1, i + 1] = Model.Columns[i];
            }
        }

        public override void Consume(object sender, GetEntityEventArgs args)
        {
            for (int i = 0; i < Model.Columns.Count(); ++i)
            {
                object value = " ";
                if (args.CurrentEntity.TryGetValue<object>(Model.Columns[i], out value))
                {
                    _WorkSheet.Cells[args.Index + 2, i + 1] = value.ToString();
                }
            }
        }

        public override void Finish(object sender, FinishEventArgs args)
        {
            // Set the whole sheet as table style
            string lastCellName = ColumnNumberToName(Model.Columns.Count()) + (args.Count + 1);
            FormatAsTable((Range)_WorkSheet.get_Range("A1", lastCellName));

            SaveExcel();
        }


        private void SaveExcel()
        {
            _Workbook.SaveAs(Path, Excel.XlFileFormat.xlWorkbookDefault, MissValue, MissValue, MissValue, MissValue, Excel.XlSaveAsAccessMode.xlExclusive, MissValue, MissValue, MissValue, MissValue, MissValue);
            _Workbook.Close(true, MissValue, MissValue);
            _Application.Quit();
        }

        private void InitExcel()
        {
            _Application = new Excel.Application();
            _Workbook = _Application.Workbooks.Add();
            _WorkSheet = _Workbook.Worksheets.get_Item(1);
        }

        private void FormatAsTable(Excel.Range SourceRange, string TableStyleName = "TableStyleLight9")
        {
            string TableName = "TableName";
            SourceRange.Worksheet.ListObjects.Add(XlListObjectSourceType.xlSrcRange,
                                                  SourceRange,
                                                  System.Type.Missing,
                                                  XlYesNoGuess.xlYes,
                                                  System.Type.Missing).Name = TableName;

            SourceRange.Worksheet.ListObjects[TableName].TableStyle = TableStyleName;
        }

        private string ColumnNumberToName(int col_num)
        {
            // See if it's out of bounds.    
            if (col_num < 1) return "A";
            // Calculate the letters.    
            string result = "";
            while (col_num > 0)
            {
                // Get the least significant digit.        
                col_num -= 1;
                int digit = col_num % 26;
                // Convert the digit into a letter.        
                result = (char)((int)'A' + digit) + result;
                col_num = (int)(col_num / 26);
            }
            return result;
        }
    }
}
