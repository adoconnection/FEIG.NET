using System;

namespace FeigDotNet.Connections
{
    public abstract class FeigReaderConnection : IDisposable
    {
        public abstract void Open();
        public abstract void Close();

        public void Dispose()
        {
            this.Close();
        }
    }
}