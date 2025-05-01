namespace ModbusTcpPoller.Modbus;

public class ModbusDataModel
{
    public int Address { get; set; } // The starting address of the data
    public List<ushort> Values { get; set; } // The list of 16-bit register values

    public ModbusDataModel(int address, List<ushort> values)
    {
        Address = address;
        Values = values ?? throw new ArgumentNullException(nameof(values));
    }

    public override string ToString()
    {
        return $"Address: {Address}, Values: [{string.Join(", ", Values)}]";
    }
}