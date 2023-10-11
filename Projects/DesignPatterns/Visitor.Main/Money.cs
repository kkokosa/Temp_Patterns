public class Money
{
    private decimal _amount;
    private Currency _currency;

    public Money(decimal amount, Currency currency)
    {
        _amount = amount;
        _currency = currency;
    }

    public static Money operator +(Money left, Money right)
    {
        if (left._currency != right._currency)
            throw new MoneyOperationException();
        return new Money(left._amount + right._amount, left._currency);
    }

    public static bool operator ==(Money lhs, Money rhs)
        => lhs.Currency == rhs.Currency && lhs.Amount == rhs.Amount;

    public static bool operator !=(Money lhs, Money rhs)
        => !(lhs == rhs);

    public decimal Amount => _amount;
    public Currency Currency => _currency;
}

public enum Currency
{
    PLN,
    EUR,
    USD,
    GBP
}

public class MoneyOperationException : Exception
{
}