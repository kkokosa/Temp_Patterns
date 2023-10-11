namespace Reports.Main.Bridge
{
    public interface ISpreadsheetEngine
    {
        void Initialize();
        void AddSpreadsheet(string title);
        void AddSheet(string title);
        void SetCellValue(int row, int column, object value);
        void Save();
        void Close();
    }
}
