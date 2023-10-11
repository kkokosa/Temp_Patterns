namespace Reports.Main.Strategy;

public interface ISalesRepository
{
    public List<Sale> GetSales(DateTime from, DateTime to);
}