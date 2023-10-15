using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing; // Make sure you have the necessary using directive.
using Laundry_Management_System;
using Laundry_Management_System.Models;
using System.Security.Policy;
using Laundry_Management_System.Data;
using System.Text;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Laundry_Management_System.Areas.Identity.Data;

public class UnitTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    

    public UnitTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        
    }

    [Theory]
    [InlineData("arpit")]
    [InlineData("ARPIT")]
    public void Test_ValidFirstNames(string FirstName)
    {
        Assert.True(ApplicationUser.IsValidFirstName(FirstName));
    }

    [Theory]
    [InlineData("Arpit123")] //Spaces should not be accepted
    [InlineData("arpit!")] //The only symbols accepted are . and _
    [InlineData("arpitsharmaarpitsharmaarpitsharmaarpitsharmaarpitsharmaarpitsharmaarpitsharma")] //55 characters is too long
    public void Test_InvalidFirstNames(string FirstName)
    {
        Assert.False(ApplicationUser.IsValidFirstName(FirstName));
    }

    [Theory]
    [InlineData("sharma")]
    [InlineData("SHARMA")]
    public void Test_ValidLastNames(string LastName)
    {
        Assert.True(ApplicationUser.IsValidLastName(LastName));
    }

    [Theory]
    [InlineData("Sharma123")] //Spaces should not be accepted
    [InlineData("sharma!!")] //The only symbols accepted are . and _
    [InlineData("arpitsharmaarpitsharmaarpitsharmaarpitsharmaarpitsharmaarpitsharmaarpitsharma")] //55 characters is too long
    public void Test_InvalidLastNames(string LastName)
    {
        Assert.False(ApplicationUser.IsValidLastName(LastName));
    }

    [Theory]
    [InlineData("test.com")] //No http or https
    [InlineData("test@testcom")]
    [InlineData("test.com@ttest")]
    [InlineData("testtets@.com")]
        public void Test_InvalidEmail(string URL)
    {
        Assert.False(ApplicationUser.IsValidEmail(URL));
    }

    [Theory]
    [InlineData("abc@abc.com")]
    [InlineData("test@test.com")]
    
    public void Test_ValidEmail(string URL)
    {
        Assert.True(ApplicationUser.IsValidEmail(URL));
    }

    [Theory]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789012!@#!#!@#@!$!")]
    [InlineData("qwe12")]
    public void Test_InvalidPasswords(string Password)
    {
        Assert.False(ApplicationUser.IsValidPassword(Password));
    }

    [Theory]
    [InlineData("vnsfjdk9e34HBCD890csdhjcds&*^&^&CDS")]
    [InlineData("hjcsdhjbdsj")]
    [InlineData("HBCJHDSBC")]
    [InlineData("8998878786768687")]
    [InlineData("&^*&%&%^$%%&^^*&&")]
    public void Test_ValidPasswords(string Password)
    {
        Assert.True(ApplicationUser.IsValidPassword(Password));
    }


    [Theory]
    [InlineData("12321832137129371298sajdjadlkajk")]
    [InlineData("-100")]
    public void IsInvalidBalance(string balance)
    {
        Assert.False(EWallet.IsValidBalance(balance));
    }

    [Theory]
    [InlineData("11")]
    [InlineData("100")]
    public void IsValidBalance(string balance)
    {
        Assert.True(EWallet.IsValidBalance(balance));
    }

    [Theory]
    [InlineData("312344")]
    [InlineData("123456")]
    public void IsValidAccountNumber(string accountnumber)
    {
        Assert.True(EWallet.IsValidAccountNumber(accountnumber));
    }

    [Theory]
    [InlineData("3123443234234")]
    [InlineData("122@#_")]
    public void IsInvalidAccountNumber(string accountnumber)
    {
        Assert.False(EWallet.IsValidAccountNumber(accountnumber));
    }
    /*
    

    [Fact]
    public async Task SuccessfulLoginTest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var loginUrl = "/Identity/Account/Login"; // Adjust the URL based on your application's routing.

        var loginData = new Dictionary<string, string>
    {
        { "Input.Email", "test@test.com" }, // Provide valid email and password
        { "Input.Password", "abc@123" },
        { "Input.RememberMe", "false" } // Adjust as needed
    };

        var content = new FormUrlEncodedContent(loginData);

        // Act
        var response = await client.PostAsync(loginUrl, content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Check for a successful login response
    }

    [Fact]
    public async Task UnsuccessfulLoginTest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var loginUrl = "/Identity/Account/Login"; // Adjust the URL based on your application's routing.

        var loginData = new Dictionary<string, string>
    {
        { "Input.Email", "test@test.com" }, // Provide valid email but incorrect password
        { "Input.Password", "abc@12" },
        { "Input.RememberMe", "false" } // Adjust as needed
    };

        var content = new FormUrlEncodedContent(loginData);

        // Act
        var response = await client.PostAsync(loginUrl, content);

        // Assert
        Assert.NotEqual(HttpStatusCode.OK, response.StatusCode); // Check for an unsuccessful login response
    }*/


    // Add your test methods here
}
