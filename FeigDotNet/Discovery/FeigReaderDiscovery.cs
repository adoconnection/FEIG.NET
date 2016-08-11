using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using FeigDotNet.Connections;

namespace FeigDotNet.Discovery
{
    public class FeigReaderDiscovery
    {
        public IList<NetworkInterface> ListNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(
                    ni =>
                        ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        ni.GetIPProperties().MulticastAddresses.Any() &&
                        ni.SupportsMulticast &&
                        ni.OperationalStatus == OperationalStatus.Up &&
                        ni.GetIPProperties().GetIPv4Properties() != null
                    )
                .ToList();
        }

        public IDictionary<NetworkInterface, List<FeigReaderInfo>> FindReaders(IEnumerable<NetworkInterface> networkInterfaces, int timeout = 1000)
        {
            return networkInterfaces
                .AsParallel()
                .Select(ni =>
                {
                    using (Socket sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                    {
                        sendSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, (int) IPAddress.HostToNetworkOrder(ni.GetIPProperties().GetIPv4Properties().Index));
                        sendSocket.ReceiveTimeout = 200;

                        IPEndPoint destinationEndpoint = new IPEndPoint(IPAddress.Parse("224.0.36.50"), 50000);
                        EndPoint localEndpoint = new IPEndPoint(IPAddress.Any, 0);

                        var discoveryCommandBytes = new byte[5] {0x01, 0x00, 0x00, 0x1c, 0x9b};

                        sendSocket.SendTo(discoveryCommandBytes, 0, 5, SocketFlags.None, destinationEndpoint);

                        try
                        {
                            byte[] receiveBytes = new Byte[256];
                            int length = sendSocket.ReceiveFrom(receiveBytes, ref localEndpoint);

                            return new
                            {
                                networkInterface = ni,
                                buffer = receiveBytes,
                                length
                            };
                        }
                        catch (SocketException)
                        {
                            return null;
                        }
                    }
                })
                .Where(r =>
                    r != null &&
                    r.length >= 30 &&
                    r.buffer[0] == 0x01 &&
                    r.buffer[1] == 0x00 &&
                    ((r.buffer[3] & 0x04) == 0x04))
                .Select(r => new
                {
                    r.networkInterface,
                    readerInfo = this.Parse(r.buffer)
                })
                .ToList()
                .GroupBy(k => k.networkInterface)
                .ToDictionary(k => k.Key, v => v.Select( r => r.readerInfo).ToList());
        }

        private FeigReaderInfo Parse(byte[] buffer)
        {
            FeigReaderInfo readerInfo = new FeigReaderInfo();

            readerInfo.DeviceID = buffer.Skip(6).Take(4).ToArray();
            readerInfo.IPAddress = string.Format("{0}.{1}.{2}.{3}", buffer[16], buffer[17], buffer[18], buffer[19]);

            FeigReaderType feigReaderType;

            if (!Enum.TryParse(buffer[5].ToString(), out feigReaderType))
            {
                feigReaderType = FeigReaderType.Undefined;
            }

            readerInfo.Type = feigReaderType;

            if ((buffer[3] & 0x02) == 0x02)
            {
                readerInfo.MacAddress = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", buffer[10].ToString("X2"), buffer[11].ToString("X2"), buffer[12].ToString("X2"), buffer[13].ToString("X2"), buffer[14].ToString("X2"), buffer[15].ToString("X2"));
            }
            else
            {
                readerInfo.MacAddress = "00-00-00-00-00-00";
            }

            readerInfo.DHCP = (buffer[4] & 0x80) == 0x80;

            if ((buffer[3] & 0x08) == 0x08)
            {
                readerInfo.MacAddress = string.Format("{0}.{1}.{2}.{3}", buffer[20], buffer[21], buffer[22], buffer[23]);
            }

            if ((buffer[3] & 0x10) == 0x10)
            {
                readerInfo.GatewayAddress = string.Format("{0}.{1}.{2}.{3}", buffer[24], buffer[25], buffer[26], buffer[27]);
            }

            if ((buffer[3] & 0x20) == 0x20)
            {
                readerInfo.Port = BitConverter.ToInt16(buffer.Skip(28).Take(2).Reverse().ToArray(), 0);
            }

            return readerInfo;
        }
    }
}