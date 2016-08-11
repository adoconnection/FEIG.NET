using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using FeigDotNet;
using FeigDotNet.Configuration;
using FeigDotNet.Connections;
using FeigDotNet.Discovery;
using FeigDotNet.Readers;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FeigReaderDiscovery discovery = new FeigReaderDiscovery();

            IList<NetworkInterface> networkInterfaces = discovery.ListNetworkInterfaces();
            IDictionary<NetworkInterface, List<FeigReaderInfo>> pairs = discovery.FindReaders(networkInterfaces);

            foreach (KeyValuePair<NetworkInterface, List<FeigReaderInfo>> pair in pairs)
            {
                Console.WriteLine("Network interface name: " + pair.Key.Name);

                foreach (FeigReaderInfo readerInfo in pair.Value)
                {
                    Console.WriteLine(" - " + readerInfo.Type + " - " + ArrayToString(readerInfo.DeviceID) + " - " + readerInfo.IPAddress);
                }
            }

            Console.WriteLine("\n\n");

            using (FeigReaderTcpConnection connection = new FeigReaderTcpConnection("192.168.1.125", 10001)) 
            {
                LRU1002Reader reader = new LRU1002Reader(connection);
                //UpdateConfiguration(reader);

                while (true)
                {
                    DateTime startDateTime = DateTime.Now;
                    IList<FeigTag> tags = reader.Inventory(FeigReaderAntenna.Antenna1, FeigReaderAntenna.Antenna4);
                    DateTime endDateTime = DateTime.Now;

                    foreach (FeigTag tag in tags)
                    {
                        Console.WriteLine("Ant. " + tag.Antenna + " " + tag.RSSI + " dBm, " + ArrayToString(tag.SerialNumber) + " - " + ArrayToString(tag.Handle));
                    }

                    Console.WriteLine("in " + (endDateTime - startDateTime).TotalMilliseconds);

                    Console.ReadKey();
                    Console.Clear();
                }
               
            }

            Console.ReadKey();
        }

        static void UpdateConfiguration(LRU1002Reader reader)
        {
            reader.InterfaceMode.ReaderMode = FeigReaderMode.HostMode;
            reader.RFInterface.Antenna1Power = 1.1;
            reader.RFInterface.Antenna2Power = 1.2;
            reader.RFInterface.Antenna3Power = 1.3;
            reader.RFInterface.Antenna4Power = 1.4;
            
            reader.ApplyConfigurationChanges();
        }

        static string ArrayToString(byte[] array)
        {
            StringBuilder hex = new StringBuilder();

            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }
    }

}