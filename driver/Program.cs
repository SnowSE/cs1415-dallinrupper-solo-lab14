using MathNet.Numerics.Distributions;
namespace Bank;

public class Program
{
    static DateTime startTime = new DateTime(1970, 1, 1, 8, 0, 0);
    public static void Main()
    {

        Console.WriteLine("Let's play Bank!");
        ICustomerSource customerSource = new RandomCustomerSource(
            intervalDistribution: new ContinuousUniform(0, 5),
            helpDistribution: new ContinuousUniform(0, 20)
        );

        // use the VSCode Extension "SandDance" to visualize the results
        File.WriteAllLines("maxWaitTimes.csv",
            Enumerable.Range(0, 10000)
                .Select(_ => new BankSimulation(customerSource, startTime, TimeSpan.FromHours(8), 5).Run())
                .Select(time => time.TotalHours.ToString()).ToArray());
    }

}
