namespace FeigDotNet.Configuration
{
    public enum ReaderMode
    {
        HostMode = 0x00,
        ScanMode = 0x01,
        BufferedMode = 0x80,
        NotificationMode = 0xC0
    }
}