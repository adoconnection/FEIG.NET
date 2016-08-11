using FeigDotNet.Configuration;
using FeigDotNet.Connections;

namespace FeigDotNet.Readers
{
    public abstract class LRUReader
    {
        public FeigReaderConnection Connection { get; }

        protected LRUReader(FeigReaderConnection connection)
        {
            this.Connection = connection;
        }
    }
}