using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeigDotNet.Connections;
using FeigDotNet.Readers;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FeigReaderTcpConnection connection = new FeigReaderTcpConnection("192.168.1.125", 10001))
            {
                LRU1002Reader reader = new LRU1002Reader(connection);

                IList<byte[]> tags = reader.Inventory();

                foreach (byte[] serialNumberBytes in tags)
                {
                    Console.WriteLine(ArrayToString(serialNumberBytes));
                }
            }

            Console.ReadKey();
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