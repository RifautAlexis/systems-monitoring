using ModbusTcpPoller.Modbus;

namespace ModbusTcpPoller.Polling;

public class PollingService
{
    private readonly IModbusClient _modbusClient;
    private readonly int _pollingIntervalMilliseconds;
    private Timer _timer;

    public PollingService(IModbusClient modbusClient, int pollingIntervalMilliseconds)
    {
        _modbusClient = modbusClient ?? throw new ArgumentNullException(nameof(modbusClient));
        _pollingIntervalMilliseconds = pollingIntervalMilliseconds;
    }

    public void Start()
    {
        _timer = new Timer(async _ => await PollDataAsync(), null, 0, _pollingIntervalMilliseconds);
    }

    public void Stop()
    {
        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();
    }

    private async Task PollDataAsync()
    {
        try
        {
            var data = await _modbusClient.ReadDataAsync();
            ProcessData(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during polling: {ex.Message}");
            // Log the error using a Logger utility if available
        }
    }

    private void ProcessData(object data)
    {
        // Process the polled data (e.g., save to database, send to another system)
        Console.WriteLine($"Data polled: {data}");
    }
}