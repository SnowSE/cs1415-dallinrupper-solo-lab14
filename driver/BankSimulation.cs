namespace Bank;

public class BankSimulation
{
    public DateTime CurrentTime { get; private set; }
    public readonly DateTime ClosingTime;
    public int AvailableTellers { get; private set; }
    public TimeSpan MaxWaitTime { get; private set; }
    public IEnumerable<Customer> WaitingCustomers => waitingCustomers.ToArray(); // read-only access
    private Queue<Customer> waitingCustomers = new();
    private ICustomerSource customerSource;
    private PriorityQueue<Action, DateTime> pendingEventsByTimestamp = new();

    public BankSimulation(ICustomerSource customerSource, DateTime openingTime, TimeSpan businessHours, int numberOfTellers)
    {
        AvailableTellers = numberOfTellers;
        this.customerSource = customerSource;
        this.CurrentTime = openingTime;
        this.ClosingTime = openingTime.Add(businessHours);
        EnqueueNextArrival();
    }

    protected void HandleArrival(Customer customer)
    {
        waitingCustomers.Enqueue(customer);
        TryToMoveCustomerFromLineToTeller();
        EnqueueNextArrival();
    }

    protected void HandleDeparture(Customer customer)
    {
        AvailableTellers++;
        TryToMoveCustomerFromLineToTeller();
    }

    private void EnqueueNextArrival()
    {
        if (customerSource.HasNext)
        {
            Customer nextCustomer = customerSource.Next(CurrentTime);

            if (nextCustomer.Arrival < ClosingTime)
            {
                pendingEventsByTimestamp.Enqueue(() => HandleArrival(nextCustomer), nextCustomer.Arrival);
            }
        }
    }

    private void TryToMoveCustomerFromLineToTeller()
    {
        if (waitingCustomers.Count > 0 && AvailableTellers > 0)
        {
            Customer customer = waitingCustomers.Dequeue();
            TimeSpan timeSpentInLine = CurrentTime - customer.Arrival;
            MaxWaitTime = TimeSpan.FromTicks(Math.Max(MaxWaitTime.Ticks, timeSpentInLine.Ticks));
            AvailableTellers--;

            pendingEventsByTimestamp.Enqueue(() => HandleDeparture(customer), CurrentTime + customer.HelpNeeded);
        }
    }

    public bool Step()
    {
        if (pendingEventsByTimestamp.TryDequeue(out Action? nextEvent, out DateTime eventstart))
        {
            CurrentTime = eventstart;
            nextEvent();
            return true;
        }
        return false;
    }

    public TimeSpan Run()
    {
        while (Step()) ;

        return MaxWaitTime;
    }

}