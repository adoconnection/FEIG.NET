using System;

namespace FeigDotNet.Connections
{
    public abstract class FeigReaderConnection : IDisposable
    {
        public abstract void Open();
        public abstract void Close();

        public abstract byte[] SendAndRecieve(params byte[] data);
        public abstract void Send(params byte[] data);
        public abstract byte[] Recieve();

        public void Dispose()
        {
            this.Close();
        }

        protected byte[] FastCRC16(byte[] buffer, int startIndex, int length)
        {
            uint crc = 0xFFFF;              // initial CRC value
            uint CRC_POLYNOM = 0x8408;      // CRC constant

            for (int i = startIndex; i < length; i++)
            {
                crc ^= buffer[i];

                for (int j = 0; j < 8; j++)
                {
                    uint tmp = crc & 0x0001;

                    if (tmp == 0x0001)
                    {
                        crc = (crc >> 1) ^ CRC_POLYNOM;
                    }
                    else
                    {
                        crc = (crc >> 1);
                    }
                }
            }

            return BitConverter.GetBytes(crc);
        }
    }
}