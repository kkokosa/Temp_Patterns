using OfficeOpenXml;
using Reports.Main.Strategy;

namespace Reports.Console.Strategy.Strategies;

internal class ExcelStrategy : ISpreadsheetStrategy
{
    public void Export(string title, List<List<object>> data)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var excel = new ExcelPackage();

        for (int i = 0; i < data.Count; i++)
        for (int j = 0; j < data[i].Count; j++)
        {
            int row = i + 1;
            object value = data[i][j].ToString();
            excel.Workbook.Worksheets.Add(title).Cells[row, j].Value = value;
        }
        var excelFile = new FileInfo($"{title}.xlsx");
        excel.SaveAs(excelFile);
    }
}