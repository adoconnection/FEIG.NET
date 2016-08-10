using System;
using System.Linq;
using FeigDotNet.Connections;

namespace FeigDotNet.Configuration
{
    public abstract class FeigReaderConfiguration
    {
        private readonly FeigReaderTcpConnection connection;

        protected abstract byte Address { get; }

        protected byte[] DataBytes { get; private set; }

        protected FeigReaderConfiguration(FeigReaderTcpConnection connection)
        {
            this.connection = connection;
            this.Read();
        }

        protected void SetShort(int index, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            this.DataBytes[index] = bytes[1]; // endian reverse
            this.DataBytes[index + 1] = bytes[0];
        }

        protected short GetShort(int index)
        {
            return BitConverter.ToInt16(this.DataBytes.Skip(index).Take(2).Reverse().ToArray(), 0);
        }

        protected virtual void Read()
        {
            this.DataBytes = this.connection.SendAndRecieve(0xFF, 0x80, this.Address).Skip(3).ToArray();
        }

        protected virtual void Write()
        {
            byte[] message = new byte[this.DataBytes.Length + 3];
            Array.Copy(this.DataBytes, 0, message, 3, this.DataBytes.Length);

            message[0] = 0xff;
            message[1] = 0x81;
            message[2] = this.Address;

            this.connection.SendAndRecieve(message);
            
        }
    }
}