namespace Reports.Main.Bridge;

public interface ISalesRepository
{
    public List<Sale> GetSales(DateTime from, DateTime to);
}