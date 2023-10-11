using System;

namespace ChainOfResponsibility
{
    public class FtpResourceSource : ResourceSource
    {
        protected override bool CanHandle(string handle) => false;

        protected override string InternalAcquire(string handle)
        {
            throw new NotImplementedException();
        }
    }
}