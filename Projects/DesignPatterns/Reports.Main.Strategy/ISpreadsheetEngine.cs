namespace Reports.Main.Strategy
{
    public interface ISpreadsheetStrategy
    {
        void Export(string title, List<List<object>> data);
    }
}
