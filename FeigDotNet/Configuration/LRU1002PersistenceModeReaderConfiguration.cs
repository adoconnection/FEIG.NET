using FeigDotNet.Connections;

namespace FeigDotNet.Configuration
{
    public class LRU1002PersistenceModeReaderConfiguration
    {
        private readonly FeigReaderConfigurationBank configurationBank;

        public LRU1002PersistenceModeReaderConfiguration(FeigReaderConnection connection)
        {
            this.configurationBank = new FeigReaderConfigurationBank(connection, 0x90);
        }

        public bool MergeAntennas
        {
            get
            {
                return this.configurationBank.GetByte(0) == 0x00;
            }
            set
            {
                this.configurationBank.SetByte(0, (byte)(value ? 0x00 : 0x01));
            }
        }

        public short Antenna1ResetTime
        {
            get
            {
                return this.configurationBank.GetShort(2);
            }
            set
            {
                this.configurationBank.SetShort(2, value);
            }
        }

        public short Antenna2ResetTime
        {
            get
            {
                return this.configurationBank.GetShort(4);
            }
            set
            {
                this.configurationBank.SetShort(4, value);
            }
        }

        public short Antenna3ResetTime
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

        public short Antenna4ResetTime
        {
            get
            {
                return this.configurationBank.GetShort(8);
            }
            set
            {
                this.configurationBank.SetShort(8, value);
            }
        }

        public void Write()
        {
            this.configurationBank.Write();
        }
    }
}