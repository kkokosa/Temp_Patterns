namespace Reports.Main.Bridge.V2;

public interface ISalesRepository
{
    public List<Sale> GetSales(DateTime from, DateTime to);
}