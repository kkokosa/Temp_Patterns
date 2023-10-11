using OfficeOpenXml;
using Reports.Main.Bridge;

namespace Reports.Console.Bridge.Engines
{
    internal class ExcelEngine : ISpreadsheetEngine
    {
        private string _filePath;
        private ExcelPackage _excel;
        private ExcelWorksheet _ws;

        public void Initialize()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excel = new ExcelPackage();
        }

        public void AddSpreadsheet(string title)
        {
            _filePath = title;
        }

        public void AddSheet(string title)
        {
            _ws = _excel.Workbook.Worksheets.Add(title);
        }

        public void SetCellValue(int row, int column, object value)
        {
            _ws.Cells[row, column].Value = value;
        }

        public void Save()
        {
            FileInfo excelFile = new FileInfo($"{_filePath}.xlsx");
            _excel.SaveAs(excelFile);
        }

        public void Close()
        {
            _excel.Dispose();
        }
    }
}
