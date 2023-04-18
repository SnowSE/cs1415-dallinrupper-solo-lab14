namespace Bank;

public record Customer(DateTime Arrival, TimeSpan HelpNeeded)
{
    public static int NextCustomerId { get; set; } = 0;
    public int Id { get; } = NextCustomerId++;
}
