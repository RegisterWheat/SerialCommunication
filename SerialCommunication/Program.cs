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
        var input = Reader.ReadLine();
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
Thread thread = new(new ThreadStart(WriteLoop));
thread.Start();
while (is_running)
{
    try
    {
        var input = port.ReadByte();
        Console.Write((char)input);
    }
    catch (TimeoutException)
    {
        is_running = false;
    }
}
thread.Join();
void WriteLoop()
{
    while (is_running)
    {
        try
        {
            var input = Reader.ReadLine(1000);
            if (input == "stop")
            {
                is_running = false;
            }
            port.Write(input);
        }
        catch (Exception) { }
    }
}
// https://stackoverflow.com/questions/57615/how-to-add-a-timeout-to-console-readline
class Reader
{
    private static Thread inputThread;
    private static AutoResetEvent getInput, gotInput;
    private static string input;

    static Reader()
    {
        getInput = new AutoResetEvent(false);
        gotInput = new AutoResetEvent(false);
        inputThread = new Thread(reader);
        inputThread.IsBackground = true;
        inputThread.Start();
    }

    private static void reader()
    {
        while (true)
        {
            getInput.WaitOne();
            input = Console.ReadLine();
            gotInput.Set();
        }
    }

    // omit the parameter to read a line without a timeout
    public static string ReadLine(int timeOutMillisecs = Timeout.Infinite)
    {
        getInput.Set();
        bool success = gotInput.WaitOne(timeOutMillisecs);
        if (success)
            return input;
        else
            throw new TimeoutException("User did not provide input within the timelimit.");
    }
}
