# FEIG.NET
Pure C# client for FEIG LRU readers

Inventory sample:

```cs
using (FeigReaderTcpConnection connection = new FeigReaderTcpConnection("192.168.1.125", 10001))
{
    LRU1002Reader reader = new LRU1002Reader(connection);

    IList<FeigTag> tags = reader.Inventory(FeigReaderAntenna.Antenna1, FeigReaderAntenna.Antenna2);

    foreach (FeigTag tag in tags)
    {
        // process
        // tag.Antenna
        // tag.SerialNumber
        // tag.RSSI
    }
}

```

Discovery sample:

```cs
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
```


Configuration sample:

```cs
using (FeigReaderTcpConnection connection = new FeigReaderTcpConnection("192.168.1.125", 10001))
{
    LRU1002Reader reader = new LRU1002Reader(connection);

    reader.InterfaceMode.ReaderMode = FeigReaderMode.HostMode;
    reader.RFInterface.Antenna1Power = 1.1;
    reader.RFInterface.Antenna2Power = 1.2;
    reader.RFInterface.Antenna3Power = 1.3;
    reader.RFInterface.Antenna4Power = 1.4;
            
    reader.ApplyConfigurationChanges();
}

```
