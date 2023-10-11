namespace RoleObject
{
    public class Testing
    {
        public static void DoTest()
        {
            var c1 = new CustomerCore("Konrad");
            c1.AddRole(new Borrower(c1));

            // Do some common stuff
            c1.DoSomeCommonStuff();

            // Treat as Borrower
            var borrower = c1.GetRoleOf<Borrower>() as Borrower;
            borrower?.DoSomeBorrowerStuff();

            var fullName = borrower?.FullName;
        }
    }
}