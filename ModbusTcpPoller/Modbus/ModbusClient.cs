using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTcpPoller.Modbus;

public class ModbusClient : IModbusClient
{
    private readonly string _ipAddress;
    private readonly int _port;
    private TcpClient _tcpClient;

    public ModbusClient(string ipAddress, int port)
    {
        _ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
        _port = port;
    }

    public async Task ConnectAsync()
    {
        _tcpClient = new TcpClient();
        await _tcpClient.ConnectAsync(_ipAddress, _port);
        Console.WriteLine("Connected to Modbus server.");
    }

    public async Task<object> ReadDataAsync()
    {
        if (_tcpClient == null || !_tcpClient.Connected)
        {
            throw new InvalidOperationException("Not connected to the Modbus server.");
        }

        // Example Modbus request (read holding registers)
        byte[] request = BuildModbusRequest();
        NetworkStream stream = _tcpClient.GetStream();

        // Send request
        await stream.WriteAsync(request, 0, request.Length);

        // Read response
        byte[] response = new byte[256];
        int bytesRead = await stream.ReadAsync(response, 0, response.Length);

        // Process response (this is just a placeholder)
        return ProcessModbusResponse(response, bytesRead);
    }

    public void Disconnect()
    {
        if (_tcpClient != null)
        {
            _tcpClient.Close();
            _tcpClient = null;
            Console.WriteLine("Disconnected from Modbus server.");
        }
    }

    private byte[] BuildModbusRequest()
    {
        // Example: Build a Modbus TCP request to read holding registers
        // Transaction ID (2 bytes), Protocol ID (2 bytes), Length (2 bytes), Unit ID (1 byte), Function Code (1 byte), Start Address (2 bytes), Quantity (2 bytes)
        return new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x00, 0x00, 0x00, 0x10 };
    }

    private object ProcessModbusResponse(byte[] response, int bytesRead)
    {
        // Example: Parse the response (this is just a placeholder)
        return Encoding.ASCII.GetString(response, 0, bytesRead);
    }
}