using FeigDotNet.Connections;

namespace FeigDotNet.Configuration
{
    public class LRU1002InterfaceModeReaderConfiguration : FeigReaderConfiguration
    {
        protected override byte Address
        {
            get
            {
                return 0x81;
            }
        }

        public LRU1002InterfaceModeReaderConfiguration(FeigReaderTcpConnection connection) : base(connection)
        {
        }

        public ReaderMode ReaderMode
        {
            get
            {
                return (ReaderMode) this.DataBytes[13];
            }
            set
            {
                this.DataBytes[13] = (byte) value;
            }
        }

        public short TasnponderResponseTimeout
        {
            get
            {
                return this.GetShort(6);
            }
            set {
                this.SetShort(6, value);
            }
        }


        public new void Write()
        {
            base.Write();
        }
    }
}