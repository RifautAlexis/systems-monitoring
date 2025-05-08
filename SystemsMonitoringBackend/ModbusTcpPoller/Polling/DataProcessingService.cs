using System;
using System.Collections.Generic;

namespace ModbusTcpPoller.Services;

public class DataProcessingService
{
    public void ProcessData(List<ushort> data)
    {
        if (data == null || data.Count == 0)
        {
            Console.WriteLine("No data to process.");
            return;
        }

        // Example: Log the data
        Console.WriteLine("Processing data:");
        foreach (var value in data)
        {
            Console.WriteLine($"Value: {value}");
        }

        // Placeholder: Save data to a database or send it to another system
        SaveToDatabase(data);
    }

    private void SaveToDatabase(List<ushort> data)
    {
        // Example: Simulate saving data to a database
        Console.WriteLine("Saving data to database...");
        foreach (var value in data)
        {
            Console.WriteLine($"Saved value: {value}");
        }
    }
}