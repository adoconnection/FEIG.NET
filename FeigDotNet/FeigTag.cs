namespace FeigDotNet
{
    public class FeigTag
    {
        public byte[] SerialNumber { get; set; }
        public byte[] PS { get; set; }
        public byte[] Handle { get; set; }
        public int RSSI { get; set; }
        public int Antenna { get; set; }
    }
}