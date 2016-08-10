# FEIG.NET
Pure C# client for FEIG LRU readers

Inventory Example:

```cs
using (FeigReaderTcpConnection connection = new FeigReaderTcpConnection("192.168.1.125", 10001))
{
    LRU1002Reader reader = new LRU1002Reader(connection);

    IList<byte[]> tags = reader.Inventory();

    foreach (byte[] tagSerialNumberBytes in tags)
    {
        // process tags
    }
}

```


Configuration sample:

```cs
using (FeigReaderTcpConnection connection = new FeigReaderTcpConnection("192.168.1.125", 10001))
{
    LRU1002Reader reader = new LRU1002Reader(connection);

    reader.InterfaceMode.ReaderMode = ReaderMode.HostMode;
    reader.InterfaceMode.Write();

    reader.RFInterface.AntennaPower = 1.6;
    reader.RFInterface.Write();

    reader.RFControllerReset(); // apply changes
}

```
