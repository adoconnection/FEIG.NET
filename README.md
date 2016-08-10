# FEIG.NET
Pure C# client for FEIL LRU readers

Example:

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
