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
        ~SerialCommunicationHelper()
        {
            if (port.IsOpen)
            {
                Close();
            }
        }

        public byte[] ReadAll()
        {
            var result = new byte[port.BytesToRead];
            port.Read(result, 0, port.BytesToRead);
            return result;
        }
        public int Write(byte[] data)
        {
            port.Write(data, 0, data.Length);
            return data.Length;
        }
        public void Close()
        {
            port.Close();
        }
    }
}
