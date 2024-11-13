using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace SkyStorage.Application.Users.Tests;

public class UserContextTests
{
    [Fact()]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        // arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var userValidatorMock = new Mock<IUserValidator>();

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, "108AED61-DFE6-4D21-8880-934E5552B27D"),
            new(ClaimTypes.Email, "test@mail.com"),
            new(ClaimTypes.Role, "User")
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user,
        });

        userValidatorMock.Setup(x => x.IsUserAuthenticated(user)).Returns(true);

        var userContext = new UserContext(httpContextAccessorMock.Object, userValidatorMock.Object);

        // act
        var currentUser = userContext.GetCurrentUser();

        // asset
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("108AED61-DFE6-4D21-8880-934E5552B27D");
        currentUser.Email.Should().Be("test@mail.com");
    }

    [Fact()]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
    {
        // arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var userValidatorMock = new Mock<IUserValidator>();

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null!);

        userValidatorMock.Setup(x => x.IsUserAuthenticated(null)).Returns(false);

        var userContext = new UserContext(httpContextAccessorMock.Object, userValidatorMock.Object);


        // act
        Action act = () => userContext.GetCurrentUser();

        // asset
        act.Should().Throw<InvalidOperationException>().WithMessage("User context is not present");
    }
}