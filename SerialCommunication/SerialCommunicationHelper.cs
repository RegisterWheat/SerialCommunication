using System.IO.Ports;

namespace SerialCommunication
{
    class SerialCommunicationHelper
    {
        private SerialPort port;
        private List<byte> buffer = new();//簡単のためとりあえず富豪的
        private Mutex bufMutex = new();
        public SerialCommunicationHelper(string portName, int baudRate = 9600, int writeTimeout = 500, int readTimeout = 500)
        {
            port = new()
            {
                WriteTimeout = writeTimeout,
                ReadTimeout = readTimeout,
                PortName = portName,
                BaudRate = baudRate
            };
            port.DataReceived += new SerialDataReceivedEventHandler(dataReceivedHandler);
            port.Open();
            Console.WriteLine($"successfully connected via {port.PortName}");
        }

        private void dataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            bufMutex.WaitOne();
            for (int i = 0; i < port.BytesToRead; i++)
            {
                buffer.Add((byte)port.ReadByte());
            }
            bufMutex.ReleaseMutex();
        }

        public byte[] readAll()
        {
            bufMutex.WaitOne();
            var result = buffer.ToArray();
            buffer.Clear();
            bufMutex.ReleaseMutex();
            return result;
        }
    }
}
