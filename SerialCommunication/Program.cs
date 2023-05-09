using System.IO.Ports;
using SerialCommunication;

var availablePorts = SerialPort.GetPortNames();
string portName = "COM1";
if (availablePorts.Length == 0)
{
    Console.Error.WriteLine("error: no available port found");
    Environment.Exit(1);
}
else if (availablePorts.Length == 1)
{
    portName = availablePorts[0];
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
        portName = availablePorts[idx];
        break;
    }
}

SerialCommunicationHelper helper = new(portName);
//簡易的なメインループ
//while (true)
for (int i = 0; i < 10; i++)
{
    Console.Write(System.Text.Encoding.UTF8.GetString(helper.ReadAll()));
    Thread.Sleep(300);
}
Console.WriteLine("end mainloop");

