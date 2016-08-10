# FEIG.NET
Pure C# client for FEIG LRU readers

Inventory Example:

```cs
using (FeigReaderTcpConnection connection = new FeigReaderTcpConnection("192.168.1.125", 10001))
{
    LRU1002Reader reader = new LRU1002Reader(connection);

    IList<FeigTag> tags = reader.Inventory();

    foreach (FeigTag tag in tags)
    {
        // process
        // tag.Antenna
        // tag.SerialNumber
        // tag.RSSI
    }
}

```


Configuration sample:

```cs
using (FeigReaderTcpConnection connection = new FeigReaderTcpConnection("192.168.1.125", 10001))
{
    LRU1002Reader reader = new LRU1002Reader(connection);

    reader.InterfaceMode.ReaderMode = ReaderMode.HostMode;
    reader.RFInterface.Antenna1Power = 1.1;
    reader.RFInterface.Antenna2Power = 1.2;
    reader.RFInterface.Antenna3Power = 1.3;
    reader.RFInterface.Antenna4Power = 1.4;
            
    reader.ApplyConfigurationChanges();
}

```
