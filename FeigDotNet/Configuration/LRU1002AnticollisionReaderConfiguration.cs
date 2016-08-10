using FeigDotNet.Connections;

namespace FeigDotNet.Configuration
{
    public class LRU1002AnticollisionReaderConfiguration
    {
        private readonly FeigReaderConfigurationBank configurationBank;

        public LRU1002AnticollisionReaderConfiguration(FeigReaderTcpConnection connection)
        {
            this.configurationBank = new FeigReaderConfigurationBank(connection, 0x85);
        }

        public bool AnticollisionEnabled
        {
            get
            {
                return this.configurationBank.GetByte(11) == 0x04;
            }
            set
            {
                this.configurationBank.SetByte(11, (byte) (value ? 0x04 : 0x00));
            }
        }


        public void Write()
        {
            this.configurationBank.Write();
        }
    }
}