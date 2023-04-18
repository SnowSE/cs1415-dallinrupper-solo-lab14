namespace Bank;

public interface ICustomerSource
{
    public bool HasNext { get; }
    Customer Next(DateTime timeOfLastEvent);
}
