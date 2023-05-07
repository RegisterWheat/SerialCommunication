// See https://aka.ms/new-console-template for more information
using System.IO.Ports;
SerialPort port = new();
port.WriteTimeout = 1500;
port.ReadTimeout = 1500;
var availablePorts = SerialPort.GetPortNames();
if (availablePorts.Length == 0)
{
    Console.Error.WriteLine("error: no available port found");
    Environment.Exit(1);
}
else if (availablePorts.Length == 1)
{
    port.PortName = availablePorts[0];
}
else
{
    Console.WriteLine($"{availablePorts.Length} ports found.");
    for (int i = 0; i < availablePorts.Length; i++)
    {
        Console.WriteLine($"{i}: {availablePorts[i]}");
    }
    while (true)
    {
        Console.Write("select port: ");
        var input = Console.ReadLine();
        if (input == null)
        {
            Console.Error.WriteLine("error: no valid input");
            continue;
        }
        int idx;
        try
        {
            idx = int.Parse(input);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"error: {e.Message}");
            continue;
        }
        if (idx < 0 || availablePorts.Length <= idx)
        {
            Console.Error.WriteLine("error: invalid index");
            Environment.Exit(1);
        }
        port.PortName = availablePorts[idx];
        break;
    }
}
port.Open();
Console.WriteLine($"successfully connected via {availablePorts[0]}");
bool is_running = true;
while (is_running)
{
    var input = port.ReadByte();
    Console.Write((char)input);
}
