using FeigDotNet.Connections;

namespace FeigDotNet.Discovery
{
    public class FeigReaderInfo
    {
        public byte[] DeviceID { get; set; }
        public FeigReaderType Type { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string GatewayAddress { get; set; }
        public string SubnetMask { get; set; }
        public int Port { get; set; }
        public bool DHCP { get; set; }
    }
}