using System.Text.RegularExpressions;
using Xunit;

namespace BlazorServerApp.xUnitTests
{
    public class IPv4FValidator
    {
        public bool IsMatch(string address) {
            Regex ipv4Expression = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)");
            return ipv4Expression.IsMatch(address);
        }
    }

    public class IPv4ValidatorTest
    {
        [Theory]
        [InlineData("asd!", false)]
        [InlineData("asedsdasdas!", false)]
        [InlineData("12314", false)]
        [InlineData("T@#h1s!", false)]
        [InlineData("", false)]
        [InlineData("0", false)]
        [InlineData("355.255.255.255", false)]
        [InlineData("255.255.255.255)", true)]
        [InlineData("0.0.0.0)", true)]
        [InlineData("192.0.0.1)", true)]
        [InlineData("111.11.1.1)", true)]
        public void ValidIP(string address, bool expectedResult)
        {
            IPv4FValidator IPv4FValidator = new IPv4FValidator();
            bool isValid = IPv4FValidator.IsMatch(address);
            Assert.Equal(expectedResult, isValid);
        }
    }
}
