namespace ModbusTcpPoller.Modbus;

public class ModbusDataParser
{
    public List<ushort> ParseResponse(byte[] response, int bytesRead)
    {
        if (response == null || bytesRead < 3)
        {
            throw new ArgumentException("Invalid Modbus response.");
        }

        // Skip the first 3 bytes (Transaction ID, Protocol ID, and Length)
        // The actual data starts after the Unit ID and Function Code
        int dataStartIndex = 9; // Adjust based on Modbus TCP header
        int dataLength = bytesRead - dataStartIndex;

        if (dataLength <= 0)
        {
            throw new ArgumentException("No data in Modbus response.");
        }

        var parsedData = new List<ushort>();

        // Parse the data as 16-bit unsigned integers (holding registers)
        for (int i = dataStartIndex; i < bytesRead; i += 2)
        {
            if (i + 1 >= bytesRead) break; // Ensure no out-of-bounds access
            ushort value = (ushort)((response[i] << 8) | response[i + 1]);
            parsedData.Add(value);
        }

        return parsedData;
    }
}