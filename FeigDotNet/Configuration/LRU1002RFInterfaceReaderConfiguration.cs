using System;
using FeigDotNet.Connections;

namespace FeigDotNet.Configuration
{
    public class LRU1002RFInterfaceReaderConfiguration
    {
        private readonly FeigReaderConfigurationBank configurationBank83;
        private readonly FeigReaderConfigurationBank configurationBank94;


        public LRU1002RFInterfaceReaderConfiguration(FeigReaderConnection connection)
        {
            this.configurationBank83 = new FeigReaderConfigurationBank(connection, 0x83);
            this.configurationBank94 = new FeigReaderConfigurationBank(connection, 0x94);
        }

        public bool EpcGen2Enabled
        {
            get
            {
                return this.configurationBank83.GetByte(1) == 0x10;
            }
            set
            {
                this.configurationBank83.SetByte(1, (byte) (value ? 0x10 : 0x00));
            }
        }

        public FeigRegulation Regulation
        {
            get
            {
                return (FeigRegulation) this.configurationBank83.GetByte(3);
            }
            set
            {
                this.configurationBank83.SetByte(3, (byte) value);
            }
        }

        public double Antenna1Power
        {
            get
            {
                return this.ReadAntennaPower(this.configurationBank83, 2);
            }
            set
            {
                this.WriteAntennaPower(this.configurationBank83, 2, value);
            }
        }

        public double Antenna2Power
        {
            get
            {
                return this.ReadAntennaPower(this.configurationBank94, 10);
            }
            set
            {
                this.WriteAntennaPower(this.configurationBank94, 10, value);
            }
        }

        public double Antenna3Power
        {
            get
            {
                return this.ReadAntennaPower(this.configurationBank94, 11);
            }
            set
            {
                this.WriteAntennaPower(this.configurationBank94, 11, value);
            }
        }

        public double Antenna4Power
        {
            get
            {
                return this.ReadAntennaPower(this.configurationBank94, 12);
            }
            set
            {
                this.WriteAntennaPower(this.configurationBank94, 12, value);
            }
        }

        public void Write()
        {
            this.configurationBank83.Write();
            this.configurationBank94.Write();
        }


        private void WriteAntennaPower(FeigReaderConfigurationBank feigReaderConfigurationBank, int index, double value)
        {
            double rounded = PrepareAndValidateAntennaPower(value);
            feigReaderConfigurationBank.SetByte(index, (byte) (rounded * 10 + 15));
        }

        private double ReadAntennaPower(FeigReaderConfigurationBank feigReaderConfigurationBank, int index)
        {
            return (feigReaderConfigurationBank.GetByte(index) - 15d) / 10;
        }


        private static double PrepareAndValidateAntennaPower(double value)
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

            return rounded;
        }
    }
}