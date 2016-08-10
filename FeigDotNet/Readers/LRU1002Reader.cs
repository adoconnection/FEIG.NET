using System.Collections.Generic;
using System.IO;
using FeigDotNet.Configuration;
using FeigDotNet.Connections;

namespace FeigDotNet.Readers
{
    public class LRU1002Reader : LRUReader
    {
        public LRU1002InterfaceModeReaderConfiguration InterfaceMode { get; }
        public LRU1002RFInterfaceReaderConfiguration RFInterface { get; }

        public LRU1002Reader(FeigReaderTcpConnection connection) : base(connection)
        {
            this.InterfaceMode = new LRU1002InterfaceModeReaderConfiguration(connection);
            this.RFInterface = new LRU1002RFInterfaceReaderConfiguration(connection);
        }

        public void RFControllerReset()
        {
            this.Connection.SendAndRecieve(0xff, 0x63);
        }

        public IList<byte[]> Inventory()
        {
            byte[] data = this.Connection.SendAndRecieve(0xFF, 0xB0, 0x01, 0x00);

            MemoryStream memoryStream = new MemoryStream(data);
            memoryStream.Position = 0;

            BinaryReader reader = new BinaryReader(memoryStream);

            byte[] readBytes = reader.ReadBytes(3);

            short tagsCount = reader.ReadByte();

            IList<byte[]> tags = new List<byte[]>();

            for (short i = 0; i < tagsCount; i++)
            {
                byte[] gen = reader.ReadBytes(2);
                byte type = reader.ReadByte();
                byte[] something = reader.ReadBytes(2);

                switch (type)
                {
                    case 0x12:
                        tags.Add(reader.ReadBytes(16));
                        break;

                    case 0x0E:
                        tags.Add(reader.ReadBytes(12));
                        break;

                    case 0x20:
                        tags.Add(reader.ReadBytes(30));
                        break;
                }
            }

            return tags;
        }
    }
}