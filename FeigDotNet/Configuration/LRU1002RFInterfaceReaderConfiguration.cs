using System;
using FeigDotNet.Connections;

namespace FeigDotNet.Configuration
{
    public class LRU1002RFInterfaceReaderConfiguration : FeigReaderConfiguration
    {
        protected override byte Address
        {
            get
            {
                return 0x83;
            }
        }

        public LRU1002RFInterfaceReaderConfiguration(FeigReaderTcpConnection connection) : base(connection)
        {
        }

        public bool EpcGen2Enabled
        {
            get
            {
                return this.DataBytes[1] == 0x10;
            }
            set
            {
                this.DataBytes[1] = (byte) (value ? 0x10 : 0x00);
            }
        }

        public double AntennaPower
        {
            get
            {
                return (this.DataBytes[2] - 15d) / 10;
            }
            set
            {
                double rounded = Math.Round(value, 1);

                if (rounded < 0.1)
                {
                    throw new NotSupportedException("Min value is 0.1");
                }
                if (rounded > 2)
                {
                    throw new NotSupportedException("Max value is 2");
                }

                this.DataBytes[2] = (byte) (rounded * 10 + 15) ;
            }
        }

        public new void Write()
        {
            base.Write();
        }
    }
}