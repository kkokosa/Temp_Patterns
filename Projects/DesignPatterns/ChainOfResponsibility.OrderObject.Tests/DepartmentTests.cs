using ChainOfResponsibility.OrderObject.Orders;

namespace ChainOfResponsibility.OrderObject.Tests
{
    public class DepartmentTests
    {
        [Fact]
        public void DepartmentCanProcess_AproveOrder()
        {   
            var department = new Department(1, "Company", null);
            var order = new ApproveOrder()
            {
                Id = 2, Price = 100, Comment = "Please"
            };

            department.Process(order);

            Assert.Equal("Approved by 1 department", order.Comment);
        }

        [Fact]
        public void DepartmentCanProcess_CancelOrder()
        {
            var department = new Department(1, "Company", null);
            var order = new CancelOrder()
            {
                Id = 2,
                Price = 100
            };

            department.Process(order);
            Assert.Equal(CancelOrder.ReasonType.NotNeeded, order.Reason);
        }
    }
}