using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Exceptions
{
    public class ObjectDoesNotExistException<T> : SalesException
    {
        public ObjectDoesNotExistException(object id)
            : base($"Object of type {typeof(T)} and id {id} does not exist")
        {
        }
    }
}
