using AutoFixture;
using BlazorServerApp.Data;
using BlazorServerApp.Models.EF.NautralKey;
using BlazorServerApp.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace BlazorServerApp.xUnitTests
{
    public class CustomerServiceTest
    {
        [Fact]
        public void CustomerService_CustomerWithFirstNameEqualTo_ReturnJack() {
            var fixture = new Fixture();
            Customer Jack = fixture.Build<Customer>().With(c => c.FirstName, "Jack").Without(c => c.CustomerMemberShips).Create();
            IList<Customer> customers = new List<Customer> {
                Jack,
                fixture.Build<Customer>().With(c => c.FirstName,"John").Without(c => c.CustomerMemberShips).Create(),
                fixture.Build<Customer>().With(c => c.FirstName,"Marry" ).Without(c => c.CustomerMemberShips).Create()
            };
            var customersMock = MockDbSet.CreateDbSetMock(customers);
            var customersContextMock = new Mock<ShopDbContext>();
            customersContextMock.Setup(x => x.Customers).Returns(customersMock.Object);
            var usersService = new CustomerService(customersContextMock.Object);
            var customerWithFirstNameEqualToJack = usersService.GetCustmersWithFirstNameEqualTo("Jack");
            Assert.Equal(new List<Customer> { Jack }, customerWithFirstNameEqualToJack);
        }
    }
}
