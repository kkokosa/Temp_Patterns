using System.Collections.Generic;
using System.Linq;

namespace RoleObject
{
    public class CustomerCore : ICustomer
    {
        public string FullName { get; set; }
        public void DoSomeCommonStuff()
        {
            // TODO: Implement
        }

        public IList<CustomerRole> Roles { get; }

        public CustomerCore(string fullName) => FullName = fullName;

        public CustomerRole GetRoleOf<T>() where T : CustomerRole => Roles.FirstOrDefault(x => x.GetType() == typeof(T));

        public void AddRole(CustomerRole role) => Roles.Add(role);
        public bool RemoveRole(CustomerRole role) => Roles.Remove(role);
    }
}