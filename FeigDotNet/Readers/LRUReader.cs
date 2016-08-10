using FeigDotNet.Connections;

namespace FeigDotNet.Readers
{
    public abstract class LRUReader
    {
        public FeigReaderTcpConnection Connection { get; }

        protected LRUReader(FeigReaderTcpConnection connection)
        {
            this.Connection = connection;
        }
    }
}