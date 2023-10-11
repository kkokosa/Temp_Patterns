using System;

namespace ChainOfResponsibility
{
    public abstract class ResourceSource : IResourceSource
    {
        private ResourceSource next;

        protected abstract bool CanHandle(string handle);
        protected abstract string InternalAcquire(string handle);

        public string Acquire(string handle)
        {
            if (CanHandle(handle))
                return InternalAcquire(handle);
            if (next == null)
                throw new NotImplementedException();
            return next.Acquire(handle);
        }

        public void SetNext(ResourceSource next) => this.next = next;
    }
}