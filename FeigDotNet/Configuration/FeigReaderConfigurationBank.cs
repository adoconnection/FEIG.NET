using System;
using System.Linq;
using FeigDotNet.Connections;

namespace FeigDotNet.Configuration
{
    public class FeigReaderConfigurationBank
    {
        private readonly FeigReaderConnection connection;

        private readonly byte address;
        private byte[] dataBytes;
        private byte[] initialDataBytes;

        public bool HasChanges
        {
            get
            {
                for (int i = 0; i < this.dataBytes.Length; i++)
                {
                    if (this.dataBytes[i] != this.initialDataBytes[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public FeigReaderConfigurationBank(FeigReaderConnection connection, byte address)
        {
            this.address = address;
            this.connection = connection;

            this.Read();
        }

        public void SetByte(int index, byte value)
        {
            this.dataBytes[index] = value;
        }
        public byte GetByte(int index)
        {
            return this.dataBytes[index];
        }

        public void SetShort(int index, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            this.dataBytes[index] = bytes[1]; // endian reverse
            this.dataBytes[index + 1] = bytes[0];
        }

        public short GetShort(int index)
        {
            return BitConverter.ToInt16(this.dataBytes.Skip(index).Take(2).Reverse().ToArray(), 0);
        }

        public virtual void Read()
        {
            this.dataBytes = this.connection.SendAndRecieve(0xFF, 0x80, this.address).Skip(3).ToArray();
            this.initialDataBytes = this.dataBytes;
        }

        public virtual void Write()
        {
            if (!this.HasChanges)
            {
                return;
            }

            byte[] message = new byte[this.dataBytes.Length + 3];
            Array.Copy(this.dataBytes, 0, message, 3, this.dataBytes.Length);

            message[0] = 0xff;
            message[1] = 0x81;
            message[2] = this.address;

            this.connection.SendAndRecieve(message);

            this.initialDataBytes = this.dataBytes;
        }
    }
}