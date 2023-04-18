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

    }

    protected void HandleDeparture(Customer customer)
    {

    }

    private void EnqueueNextArrival()
    {

    }

    private void TryToMoveCustomerFromLineToTeller()
    {

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