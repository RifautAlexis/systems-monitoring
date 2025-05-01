// Configuration

using ModbusTcpPoller.Modbus;
using ModbusTcpPoller.Polling;

string ipAddress = "192.168.0.100"; // Replace with your Modbus server IP
int port = 502; // Default Modbus TCP port
int pollingIntervalMilliseconds = 5000; // Polling interval (5 seconds)

// Initialize ModbusClient
IModbusClient modbusClient = new ModbusClient(ipAddress, port);

try
{
    // Connect to the Modbus server
    await modbusClient.ConnectAsync();

    // Initialize and start the PollingService
    var pollingService = new PollingService(modbusClient, pollingIntervalMilliseconds);
    pollingService.Start();

    Console.WriteLine("Press Enter to stop polling...");
    Console.ReadLine();

    // Stop the PollingService
    pollingService.Stop();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    // Disconnect from the Modbus server
    modbusClient.Disconnect();
}