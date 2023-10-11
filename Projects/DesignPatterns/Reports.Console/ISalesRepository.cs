namespace Reports.Console;

public interface ISalesRepository
{
    public List<Sale> GetSales(DateTime from, DateTime to);
}