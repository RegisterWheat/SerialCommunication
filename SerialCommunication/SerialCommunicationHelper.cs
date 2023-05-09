using System.IO.Ports;

namespace SerialCommunication
{
    class SerialCommunicationHelper
    {
        private SerialPort port;
        public SerialCommunicationHelper(string portName, int baudRate = 9600, int writeTimeout = 500, int readTimeout = 500)
        {
            port = new()
            {
                WriteTimeout = writeTimeout,
                ReadTimeout = readTimeout,
                PortName = portName,
                BaudRate = baudRate
            };
            port.Open();
            Console.WriteLine($"successfully connected via {port.PortName}");
        }

        public byte[] readAll()
        {
            var result = new byte[port.BytesToRead];
            port.Read(result, 0, port.BytesToRead);
            return result;
        }
    }
}
