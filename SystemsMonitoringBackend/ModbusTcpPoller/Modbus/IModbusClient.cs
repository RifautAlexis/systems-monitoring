namespace ModbusTcpPoller.Modbus;

public interface IModbusClient
{
    Task ConnectAsync();
    Task<object> ReadDataAsync();
    void Disconnect();
}