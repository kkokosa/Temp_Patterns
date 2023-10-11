using System.Collections.Generic;

namespace RoleObject
{
    public interface ICustomer
    {
        // Common logic/properties
        string FullName { get; set; }
        void DoSomeCommonStuff();

        // Role handling
        IList<CustomerRole> Roles { get; }
        CustomerRole GetRoleOf<T>() where T : CustomerRole;
        void AddRole(CustomerRole role);
        bool RemoveRole(CustomerRole role);
    }
}