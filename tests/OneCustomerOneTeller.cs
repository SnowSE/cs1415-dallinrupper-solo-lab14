using Bank;
using FluentAssertions;
using NUnit.Framework;

namespace tests;

public class OneCustomerOneTeller
{
    readonly DateTime begin = new DateTime(1970, 1, 1, 8, 0, 0);
    private BankSimulation? simulation;

    [SetUp]
    public void Setup()
    {
        var customers = new EnumerableCustomerSource(new List<Customer>{
            new Customer(begin.AddMinutes(1), TimeSpan.FromMinutes(2)),
        });
        simulation = new BankSimulation(customers, begin, TimeSpan.FromHours(8), 1);
    }

    [Test]
    public void SingleStep()
    {
        simulation!.CurrentTime.Should().Be(begin);
        simulation!.Step();

        simulation.CurrentTime.Should().Be(begin.AddMinutes(1));
        simulation.WaitingCustomers.Should().BeEmpty();
        simulation.MaxWaitTime.Should().Be(TimeSpan.Zero);
    }

    [Test]
    public void FullSimulation()
    {
        simulation!.Run().Should().Be(TimeSpan.Zero);

        simulation.CurrentTime.Should().Be(begin.AddMinutes(3));
        simulation.WaitingCustomers.Should().BeEmpty();
        simulation.MaxWaitTime.Should().Be(TimeSpan.Zero);
    }
}