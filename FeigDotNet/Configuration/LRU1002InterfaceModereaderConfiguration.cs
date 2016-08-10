using FeigDotNet.Connections;

namespace FeigDotNet.Configuration
{
    public class LRU1002InterfaceModeReaderConfiguration
    {
        private readonly FeigReaderConfigurationBank configurationBank;

        public LRU1002InterfaceModeReaderConfiguration(FeigReaderTcpConnection connection)
        {
            this.configurationBank = new FeigReaderConfigurationBank(connection, 0x81);
        }

        public FeigReaderMode ReaderMode
        {
            get
            {
                return (FeigReaderMode) this.configurationBank.GetByte(13);
            }
            set
            {
                this.configurationBank.SetByte(13, (byte) value);
            }
        }

        public short TasnponderResponseTimeout
        {
            get
            {
                return this.configurationBank.GetShort(6);
            }
            set
            {
                this.configurationBank.SetShort(6, value);
            }
        }

        public void Write()
        {
            this.configurationBank.Write();
        }
    }
}