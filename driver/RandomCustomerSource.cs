using MathNet.Numerics.Distributions;
namespace Bank;

public class RandomCustomerSource : ICustomerSource
{
    public bool HasNext => true;
    private readonly IContinuousDistribution intervalDistribution;
    private readonly IContinuousDistribution helpDistribution;

    /// <param name="intervalDistribution">Probability distribution over the number of minutes between adjacent arrival events</param>
    /// <param name="helpDistribution">Probability distribution over the number of minutes of help needed by a customer</param>
    public RandomCustomerSource(IContinuousDistribution intervalDistribution, IContinuousDistribution helpDistribution)
    {

        this.intervalDistribution = intervalDistribution;
        this.helpDistribution = helpDistribution;
    }

    public Customer Next(DateTime previousTime)
    {
        return new Customer(
            Arrival: previousTime.AddMinutes(intervalDistribution.Sample()),
            HelpNeeded: TimeSpan.FromMinutes(helpDistribution.Sample()));
    }

}
