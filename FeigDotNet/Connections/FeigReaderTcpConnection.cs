using System;
using System.Net;
using System.Net.Sockets;
using FeigDotNet.Exceptions;

namespace FeigDotNet.Connections
{
    public class FeigReaderTcpConnection : FeigReaderConnection
    {
        private readonly string readerHostAddress;
        private readonly int readerPortNumber;
        private readonly int timeout;
        private Socket tcpConnection = null;
        private bool isConnected = false;

        public FeigReaderTcpConnection(string readerHostAddress, int readerPortNumber, int timeout = 1000)
        {
            this.readerHostAddress = readerHostAddress;
            this.readerPortNumber = readerPortNumber;
            this.timeout = timeout;
        }

        public override void Open()
        {
            if (this.isConnected)
            {
                throw new FeigException("Already connected");
            }

            this.tcpConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.tcpConnection.ReceiveTimeout = this.timeout;
            this.tcpConnection.SendTimeout = this.timeout;

            this.tcpConnection.Connect(new IPEndPoint(IPAddress.Parse(this.readerHostAddress), this.readerPortNumber));
            this.isConnected = true;
        }

        public override void Close()
        {
            if (!this.isConnected)
            {
                return;
            }

            this.tcpConnection.Close(2000);
            this.isConnected = false;
        }

        public override byte[] SendAndRecieve(params byte[] data)
        {
            this.Send(data);
            return this.Recieve();
        }

        public override void Send(params byte[] data)
        {
            if (!this.isConnected)
            {
                this.Open();
            }

            byte[] package = new byte[data.Length + 5]; // 3 for prefix and 2 for crc16

            Array.Copy(data, 0, package, 3, data.Length);

            byte[] sendDataLength = BitConverter.GetBytes(package.Length);
            Array.Reverse(sendDataLength);

            package[0] = 0x02;
            package[1] = sendDataLength[2];
            package[2] = sendDataLength[3];

            byte[] crc16 = this.FastCRC16(package, 0, data.Length + 3); // all but 2 last bytes

            
            package[data.Length + 3] = crc16[0];
            package[data.Length + 4] = crc16[1];

            try
            {
                int res = this.tcpConnection.Send(package);
            }
            catch (SocketException e)
            {
                throw new FeigConnectionException("Unable to send data", e);
            }
        }

        public override byte[] Recieve()
        {
            if (!this.isConnected)
            {
                throw new FeigException("Connection closed");
            }

            try
            {
                byte[] responseTypeBuffer = new byte[1];
                this.tcpConnection.Receive(responseTypeBuffer);

                if (responseTypeBuffer[0] != 0x02)
                {
                    throw new FeigConnectionException("Invalid reader response");
                }

                byte[] dataLengthBuffer = new byte[2];
                this.tcpConnection.Receive(dataLengthBuffer);

                Array.Reverse(dataLengthBuffer);

                short dataLength = BitConverter.ToInt16(dataLengthBuffer, 0);

                byte[] dataBuffer = new byte[dataLength - 5];
                this.tcpConnection.Receive(dataBuffer);

                byte[] signatureBuffer = new byte[2];
                this.tcpConnection.Receive(signatureBuffer);

                return dataBuffer;
            }
            catch (SocketException e)
            {
                throw new FeigConnectionException("Unable to recieve data", e);
            }
        }
    }
}