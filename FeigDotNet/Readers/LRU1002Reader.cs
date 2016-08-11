using System.Collections.Generic;
using System.IO;
using System.Text;
using FeigDotNet.Configuration;
using FeigDotNet.Connections;

namespace FeigDotNet.Readers
{
    public class LRU1002Reader : LRUReader
    {
        public LRU1002InterfaceModeReaderConfiguration InterfaceMode { get; }
        public LRU1002RFInterfaceReaderConfiguration RFInterface { get; }
        public LRU1002AnticollisionReaderConfiguration Anticollision { get; }
        public LRU1002PersistenceModeReaderConfiguration PersistenceMode { get; }
        public LRU1002MultiplexerReaderConfiguration Multiplexer { get; }

        public LRU1002Reader(FeigReaderTcpConnection connection) : base(connection)
        {
            this.InterfaceMode = new LRU1002InterfaceModeReaderConfiguration(connection);
            this.RFInterface = new LRU1002RFInterfaceReaderConfiguration(connection);
            this.Anticollision = new LRU1002AnticollisionReaderConfiguration(connection);
            this.PersistenceMode = new LRU1002PersistenceModeReaderConfiguration(connection);
            this.Multiplexer = new LRU1002MultiplexerReaderConfiguration(connection);
        }

        public void ApplyConfigurationChanges()
        {
            this.InterfaceMode.Write();
            this.RFInterface.Write();
            this.Anticollision.Write();
            this.PersistenceMode.Write();
            this.Multiplexer.Write();

            this.RFControllerReset();
        }
        
        /// <summary>
        /// No RSSI and antenna info
        /// </summary>
        /// <returns></returns>
        public IList<FeigTag> Inventory()
        {
            byte[] data = this.Connection.SendAndRecieve(0xFF, 0xB0, 0x01, 0x00);

            MemoryStream memoryStream = new MemoryStream(data);
            memoryStream.Position = 0;

            BinaryReader reader = new BinaryReader(memoryStream);

            byte[] readBytes = reader.ReadBytes(3);

            short tagsCount = reader.ReadByte();

            IList<FeigTag> tags = new List<FeigTag>();

            for (short i = 0; i < tagsCount; i++)
            {
                byte[] gen = reader.ReadBytes(2);
                byte type = reader.ReadByte();
                byte[] ps = reader.ReadBytes(2);

                switch (type)
                {
                    case 0x12:
                        tags.Add(new FeigTag { SerialNumber = reader.ReadBytes(16)});
                        break;

                    case 0x0E:
                        tags.Add(new FeigTag { SerialNumber = reader.ReadBytes(12) });
                        break;

                    case 0x20:
                        tags.Add(new FeigTag { SerialNumber = reader.ReadBytes(30) });
                        break;
                }
            }

            return tags;
        }
        
        public IList<FeigTag> Inventory(params FeigReaderAntenna[] antennas)
        {
            int antennasByte = 0;

            foreach (FeigReaderAntenna antenna in antennas)
            {
                antennasByte = antennasByte | (int) antenna;
            }

            byte[] data = this.Connection.SendAndRecieve(0xFF, 0xB0, 0x01, 0x10, (byte) antennasByte);

            MemoryStream memoryStream = new MemoryStream(data);
            memoryStream.Position = 0;

            BinaryReader reader = new BinaryReader(memoryStream);

            byte[] readBytes = reader.ReadBytes(3);

            short tagsCount = reader.ReadByte();
         //   reader.ReadByte();

            IList<FeigTag> tags = new List<FeigTag>();

            for (short i = 0; i < tagsCount; i++)
            {
                byte separator = reader.ReadByte();
                byte[] gen = reader.ReadBytes(2);
                byte type = reader.ReadByte();
                byte[] ps = reader.ReadBytes(2);

                byte[] serialNumber;
                byte status;
                byte antenna;
                byte rssi;
                byte[] handle;

                switch (type)
                {
                    case 0x12:
                        serialNumber = reader.ReadBytes(16);
                        break;

                    case 0x0E:
                        serialNumber = reader.ReadBytes(12);
                        break;

                    case 0x20:
                        serialNumber = reader.ReadBytes(30);
                        break;

                    default:
                        continue;
                }

                //     a    rs
                // 01 01 00 26 00 12 00 00  
                // 01 04 00 48 01 F0 00 00 

                reader.ReadByte();
                antenna = reader.ReadByte();
                reader.ReadByte();
                rssi = reader.ReadByte();
                handle = reader.ReadBytes(2);
                reader.ReadByte();
                reader.ReadByte();

                tags.Add(new FeigTag()
                {
                    SerialNumber = serialNumber,
                    PS = ps,
                    Antenna = antenna,
                    Handle = handle,
                    RSSI = -rssi
                });

            }

            return tags;
        }

        public bool UpdateTagSerialNumber(byte[] serialNumberToChange, byte[] newSerialNumber)
        {
            MemoryStream memoryStream = new MemoryStream();

            byte[] command = {0xFF, 0xB0, 0x24, 0x31, (byte) serialNumberToChange.Length };
            byte[] separator = {0x01, 0x00, 0x01, 0x09, 0x02, 0x40, 0x00};

            memoryStream.Write(command, 0, command.Length);
            memoryStream.Write(serialNumberToChange, 0, serialNumberToChange.Length);
            memoryStream.Write(separator, 0, separator.Length);
            memoryStream.Write(newSerialNumber, 0, newSerialNumber.Length);

            memoryStream.Position = 0;

            byte[] data = this.Connection.SendAndRecieve(memoryStream.ToArray());

            return (data.Length == 3 && data[0] == 0x00 && data[1] == 0xb0 && data[2] == 0x00);
        }

        private void RFControllerReset()
        {
            this.Connection.SendAndRecieve(0xff, 0x63);
        }
    }
}