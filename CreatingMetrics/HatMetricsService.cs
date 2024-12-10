using System.Diagnostics.Metrics;

public class HatMetricsService
{
    private readonly Meter _meter;
    private readonly Counter<int> _hatsSoldCounter;

    public HatMetricsService(Meter meter)
    {
        _meter = meter;
        _hatsSoldCounter = _meter.CreateCounter<int>("hatco.store.hats_sold");
    }

    public void RecordSale(int quantity, string tipo, string cor)
    {
        _hatsSoldCounter.Add(quantity, 
            KeyValuePair.Create<string, object?>("tipo", tipo),
            KeyValuePair.Create<string, object?>("cor", cor));
    }
}
