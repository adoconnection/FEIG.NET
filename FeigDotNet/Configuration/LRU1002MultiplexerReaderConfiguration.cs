using FeigDotNet.Connections;

namespace FeigDotNet.Configuration
{
    public class LRU1002MultiplexerReaderConfiguration
    {
        private readonly FeigReaderConfigurationBank configurationBank;

        public LRU1002MultiplexerReaderConfiguration(FeigReaderTcpConnection connection)
        {
            this.configurationBank = new FeigReaderConfigurationBank(connection, 0x8F);
        }

        public bool MultiplexerEnabled
        {
            get
            {
                return this.configurationBank.GetByte(0) == 0x01;
            }
            set
            {
                this.configurationBank.SetByte(0, (byte)(value ? 0x01 : 0x00));
            }
        }

        public bool Antenna1Selected
        {
            get
            {
                return (this.configurationBank.GetByte(1) & 8) == 8;
            }
            set
            {
                this.configurationBank.SetByte(1, (byte) (128 | (value ? 8 : 0) | (this.Antenna2Selected ? 16 : 0) | (this.Antenna3Selected ? 32 : 0) | (this.Antenna4Selected ? 64 : 0)));
            }
        }

        public bool Antenna2Selected
        {
            get
            {
                return (this.configurationBank.GetByte(1) & 16) == 16;
            }
            set
            {
                this.configurationBank.SetByte(1, (byte)(128 | (this.Antenna1Selected ? 8 : 0) | (value ? 16 : 0) | (this.Antenna3Selected ? 32 : 0) | (this.Antenna4Selected ? 64 : 0)));
            }
        }

        public bool Antenna3Selected
        {
            get
            {
                return (this.configurationBank.GetByte(1) & 32) == 32;
            }
            set
            {
                this.configurationBank.SetByte(1, (byte)(128 | (this.Antenna1Selected ? 8 : 0) | (this.Antenna2Selected ? 16 : 0) | (value ? 32 : 0) | (this.Antenna4Selected ? 64 : 0)));
            }
        }

        public bool Antenna4Selected
        {
            get
            {
                return (this.configurationBank.GetByte(1) & 64) == 64;
            }
            set
            {
                this.configurationBank.SetByte(1, (byte)(128 | (this.Antenna1Selected ? 8 : 0) | (this.Antenna2Selected ? 16 : 0) | (this.Antenna3Selected ? 32 : 0) | (value ? 64 : 0)));
            }
        }


        public void Write()
        {
            this.configurationBank.Write();
        }
        
    }
}