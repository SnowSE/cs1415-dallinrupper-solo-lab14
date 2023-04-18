using Bank;
using FluentAssertions;
using NUnit.Framework;

namespace tests;

public class TwoCustomersOneTeller
{
    readonly DateTime begin = new DateTime(1970, 1, 1, 8, 0, 0);
    private BankSimulation? simulation;

    [SetUp]
    public void Setup()
    {
        var customers = new EnumerableCustomerSource(new List<Customer>{
            new Customer(begin.AddMinutes(1), TimeSpan.FromMinutes(2)),
            new Customer(begin.AddMinutes(2), TimeSpan.FromMinutes(2)),
        });
        simulation = new BankSimulation(customers, begin, TimeSpan.FromHours(8), 1);
    }

    [Test]
    public void Step01_Customer1WithTeller_Customer2NotYetArrived()
    {
        simulation!.Step();

        simulation.CurrentTime.Should().Be(begin.AddMinutes(1));
        simulation.WaitingCustomers.Should().HaveCount(0);
        simulation.MaxWaitTime.Should().Be(TimeSpan.Zero);
        simulation.AvailableTellers.Should().Be(0);
    }

    [Test]
    public void Step02_Customer1WithTeller_Customer2Arrived()
    {
        foreach (var _ in Enumerable.Range(0, 2))
            simulation!.Step();

        simulation!.CurrentTime.Should().Be(begin.AddMinutes(2));
        simulation.WaitingCustomers.Should().HaveCount(1);
        simulation.MaxWaitTime.Should().Be(TimeSpan.Zero);
        simulation.AvailableTellers.Should().Be(0);
    }

    [Test]
    public void Step03_Customer1Leaves_Customer2WithTeller()
    {
        foreach (var _ in Enumerable.Range(0, 3))
            simulation!.Step();

        simulation!.CurrentTime.Should().Be(begin.AddMinutes(3));
        simulation.WaitingCustomers.Should().HaveCount(0);
        simulation.MaxWaitTime.Should().Be(TimeSpan.FromMinutes(1));
        simulation.AvailableTellers.Should().Be(0);
    }

    [Test]
    public void Step04_Customer2Leaves()
    {
        foreach (var _ in Enumerable.Range(0, 4))
            simulation!.Step();

        simulation!.CurrentTime.Should().Be(begin.AddMinutes(5));
        simulation.WaitingCustomers.Should().HaveCount(0);
        simulation.MaxWaitTime.Should().Be(TimeSpan.FromMinutes(1));
        simulation.AvailableTellers.Should().Be(1);
    }

    [Test]
    public void FullSimulation()
    {
        simulation!.Run().Should().Be(TimeSpan.FromMinutes(1));
    }
}