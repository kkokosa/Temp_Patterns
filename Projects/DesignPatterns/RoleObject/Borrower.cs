namespace RoleObject
{
    public class Borrower : CustomerRole
    {
        private decimal credis;
        private decimal securities;

        public Borrower(CustomerCore customerCore) : base(customerCore)
        {
        }

        public void DoSomeBorrowerStuff()
        {

        }
    }
}