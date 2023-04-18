using Bank;
using FluentAssertions;
using NUnit.Framework;

namespace tests;

public class SevenCustomersTwoTellers
{
    readonly DateTime begin = new DateTime(1970, 1, 1, 8, 0, 0);
    private BankSimulation? simulation;

    [SetUp]
    public void Setup()
    {
        Customer.NextCustomerId = 1;
        var customers = new EnumerableCustomerSource(new List<Customer>{
            new Customer(begin.AddMinutes(1), TimeSpan.FromMinutes(2)),
            new Customer(begin.AddMinutes(1.5), TimeSpan.FromMinutes(1)),
            new Customer(begin.AddMinutes(2), TimeSpan.FromMinutes(3)),
            new Customer(begin.AddMinutes(2.5), TimeSpan.FromMinutes(1)),
            new Customer(begin.AddMinutes(5), TimeSpan.FromMinutes(5)),
            new Customer(begin.AddMinutes(5), TimeSpan.FromMinutes(20)),
            new Customer(begin.AddMinutes(7), TimeSpan.FromMinutes(2))
        });
        simulation = new BankSimulation(customers, begin, TimeSpan.FromHours(8), 2);
    }

    [Test]
    public void Step01_Customer1WithTeller_Customer2NotYetArrived()
    {
        simulation!.Step();

        simulation.CurrentTime.Should().Be(begin.AddMinutes(1));
        simulation.WaitingCustomers.Should().HaveCount(0);
        simulation.MaxWaitTime.Should().Be(TimeSpan.Zero);
        simulation.AvailableTellers.Should().Be(1);
    }

    [Test]
    public void Step02_01half_Customer1WithTeller_Customer2Arrived()
    {
        foreach (var _ in Enumerable.Range(0, 2))
            simulation!.Step();

        simulation!.CurrentTime.Should().Be(begin.AddMinutes(1.5));
        simulation.WaitingCustomers.Should().HaveCount(0);
        simulation.MaxWaitTime.Should().Be(TimeSpan.Zero);
        simulation.AvailableTellers.Should().Be(0);
    }

    [Test]
    public void Step03_02Min_Customer1Leaves_Customer2WithTeller_Customer3SkipsLineAndGoesStraightToTeller()
    {
        foreach (var _ in Enumerable.Range(0, 3))
            simulation!.Step();

        simulation!.CurrentTime.Should().Be(begin.AddMinutes(2));
        simulation.WaitingCustomers.Should().HaveCount(1);
        simulation.MaxWaitTime.Should().Be(TimeSpan.FromMinutes(0));
        simulation.AvailableTellers.Should().Be(0);
    }

    [Test]
    public void Step04_02half_Min_Customer2Leaves_Customer3WithTeller1()
    {
        foreach (var _ in Enumerable.Range(0, 4))
            simulation!.Step();

        simulation!.CurrentTime.Should().Be(begin.AddMinutes(2.5));
        simulation.WaitingCustomers.Should().HaveCount(0);
        simulation.MaxWaitTime.Should().Be(TimeSpan.FromSeconds(30));
        simulation.AvailableTellers.Should().Be(0);
    }

    [Test]
    public void Step05_02half_Min_Customer3Leaves_Customer4ArrivesAndGetsInLine()
    {
        foreach (var _ in Enumerable.Range(0, 5))
            simulation!.Step();

        simulation!.CurrentTime.Should().Be(begin.AddMinutes(2.5));
        simulation.WaitingCustomers.Should().HaveCount(1);
        simulation.MaxWaitTime.Should().Be(TimeSpan.FromSeconds(30));
        simulation.AvailableTellers.Should().Be(0);
    }

    [Test]
    public void Step06_02half_Min_Customer3Leaves_Customer4ArrivesAndGetsInLine()
    {
        foreach (var _ in Enumerable.Range(0, 5))
            simulation!.Step();

        simulation!.CurrentTime.Should().Be(begin.AddMinutes(2.5));
        simulation.WaitingCustomers.Should().HaveCount(1);
        simulation.MaxWaitTime.Should().Be(TimeSpan.FromSeconds(30));
        simulation.AvailableTellers.Should().Be(0);
    }

    [Test]
    public void FullSimulation()
    {
        var actualMaxTime = simulation!.Run();
        actualMaxTime.Should().Be(TimeSpan.FromMinutes(3));
    }
}