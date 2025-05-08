using Microsoft.Extensions.Logging;
using FluentModbus;

/* create logger */
var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
    loggingBuilder.AddConsole();
});

var serverLogger = loggerFactory.CreateLogger("Server");

/* create Modbus TCP server */
using var server = new ModbusTcpServer(serverLogger)
{
    // see 'RegistersChanged' event below
    EnableRaisingEvents = true
};

/* subscribe to the 'RegistersChanged' event (in case you need it) */
server.RegistersChanged += (sender, registerAddresses) =>
{
    // the variable 'registerAddresses' contains a list of modified register addresses
    serverLogger.LogInformation("Has changed: {registerAddresses}", string.Join(", ", registerAddresses));
};

/* run Modbus TCP server */
var cts = new CancellationTokenSource();
server.Start();
serverLogger.LogInformation("Server started.");

var taskServer = Task.Run(async () =>
{
    while (!cts.IsCancellationRequested)
    {
        // lock is required to synchronize buffer access between this application and one or more Modbus clients
        lock (server.Lock)
        {
            DoServerWork(server);
        }

        // update server register content once per second
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}, cts.Token);

// Wait for user input to stop the server
Console.WriteLine("Press any key to stop the server...");
Console.ReadKey();

// stop server
cts.Cancel();
await taskServer;

server.Stop();
serverLogger.LogInformation("Server stopped.");


static void DoServerWork(ModbusTcpServer server)
{
    var random = new Random();

    // Option A: normal performance version, more flexibility

    /* get buffer in standard form (Span<short>) */
    var registers = server.GetHoldingRegisters();
    registers.SetLittleEndian<int>(address: 5, random.Next());

    // Option B: high performance version, less flexibility

    /* interpret buffer as array of bytes (8 bit) */
    var byte_buffer = server.GetHoldingRegisterBuffer<byte>();
    byte_buffer[20] = (byte)(random.Next() >> 24);

    /* interpret buffer as array of shorts (16 bit) */
    var short_buffer = server.GetHoldingRegisterBuffer<short>();
    short_buffer[30] = (short)(random.Next(0, 100) >> 16);

    /* interpret buffer as array of ints (32 bit) */
    var int_buffer = server.GetHoldingRegisterBuffer<int>();
    int_buffer[40] = random.Next(0, 100);
}