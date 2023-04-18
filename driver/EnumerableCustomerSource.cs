namespace Bank;

/// <summary>Wraps an enumerable to provide the ICustomerSource interface</summary>
public class EnumerableCustomerSource : ICustomerSource
{
    public bool HasNext { get; private set; }

    private IEnumerator<Customer> enumerator;

    public EnumerableCustomerSource(IEnumerable<Customer> enumerable)
    {
        enumerator = enumerable.GetEnumerator();
        HasNext = enumerator.MoveNext();
    }

    public Customer Next(DateTime previousTime)
    {
        Customer next = enumerator.Current;
        HasNext = enumerator.MoveNext();
        return next;
    }
}
