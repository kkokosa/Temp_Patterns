using System.Collections.Generic;

namespace RoleObject
{
    public abstract class CustomerRole : ICustomer
    {
        protected readonly CustomerCore customerCore;

        public CustomerRole(CustomerCore customerCore)
        {
            this.customerCore = customerCore;
        }

        public string FullName
        {
            get => customerCore.FullName;
            set => customerCore.FullName = value; 
        }

        public void DoSomeCommonStuff()
        {
            customerCore.DoSomeCommonStuff();
        }

        public IList<CustomerRole> Roles
        {
            get => customerCore.Roles;
        }

        public CustomerRole GetRoleOf<T>() where T : CustomerRole
        {
            return customerCore.GetRoleOf<T>();
        }

        public void AddRole(CustomerRole role)
        {
            customerCore.AddRole(role);
        }
        public bool RemoveRole(CustomerRole role)
        {
            return customerCore.RemoveRole(role);
        }
    }
}