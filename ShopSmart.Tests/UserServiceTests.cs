using ShopSmart.Enums;
using ShopSmart.Services;

public class UserServiceTests : TestBase
{
    // --- Register ---

    [Fact]
    public void Register_ValidInput_ReturnsUser()
    {
        var user = UserService.Register("alice", "alice@example.com", "password123", UserRole.Customer);

        Assert.Equal("alice", user.Username);
        Assert.Equal("alice@example.com", user.Email);
        Assert.Equal(UserRole.Customer, user.Role);
        Assert.Equal(0m, user.WalletBalance);
    }

    [Fact]
    public void Register_AssignsIncrementingIds()
    {
        var u1 = UserService.Register("alice", "alice@example.com", "pass123", UserRole.Customer);
        var u2 = UserService.Register("bob",   "bob@example.com",   "pass123", UserRole.Customer);

        Assert.NotEqual(u1.Id, u2.Id);
    }

    [Fact]
    public void Register_EmptyUsername_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            UserService.Register("", "alice@example.com", "pass123", UserRole.Customer));
    }

    [Fact]
    public void Register_EmptyEmail_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            UserService.Register("alice", "", "pass123", UserRole.Customer));
    }

    [Fact]
    public void Register_EmptyPassword_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            UserService.Register("alice", "alice@example.com", "", UserRole.Customer));
    }

    [Fact]
    public void Register_DuplicateUsername_ThrowsValidationException()
    {
        UserService.Register("alice", "alice@example.com", "pass123", UserRole.Customer);

        Assert.Throws<ValidationException>(() =>
            UserService.Register("alice", "other@example.com", "pass123", UserRole.Customer));
    }

    [Fact]
    public void Register_DuplicateEmail_ThrowsValidationException()
    {
        UserService.Register("alice", "alice@example.com", "pass123", UserRole.Customer);

        Assert.Throws<ValidationException>(() =>
            UserService.Register("bob", "alice@example.com", "pass123", UserRole.Customer));
    }

    // --- Login ---

    [Fact]
    public void Login_ValidCredentials_ReturnsUser()
    {
        UserService.Register("alice", "alice@example.com", "password123", UserRole.Customer);

        var user = UserService.Login("alice", "password123");

        Assert.Equal("alice", user.Username);
    }

    [Fact]
    public void Login_WrongPassword_ThrowsValidationException()
    {
        UserService.Register("alice", "alice@example.com", "password123", UserRole.Customer);

        Assert.Throws<ValidationException>(() => UserService.Login("alice", "wrongpass"));
    }

    [Fact]
    public void Login_UnknownUsername_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() => UserService.Login("nobody", "password123"));
    }

    [Fact]
    public void Login_IsCaseInsensitiveForUsername()
    {
        UserService.Register("Alice", "alice@example.com", "pass123", UserRole.Customer);

        var user = UserService.Login("alice", "pass123");

        Assert.Equal("Alice", user.Username);
    }

    // --- TopUpWallet ---

    [Fact]
    public void TopUpWallet_ValidAmount_AddsToBalance()
    {
        var user = UserService.Register("alice", "alice@example.com", "pass123", UserRole.Customer);

        UserService.TopUpWallet(user, 100m);

        Assert.Equal(100m, user.WalletBalance);
    }

    [Fact]
    public void TopUpWallet_Cumulative_AccumulatesCorrectly()
    {
        var user = UserService.Register("alice", "alice@example.com", "pass123", UserRole.Customer);

        UserService.TopUpWallet(user, 50m);
        UserService.TopUpWallet(user, 75m);

        Assert.Equal(125m, user.WalletBalance);
    }

    [Fact]
    public void TopUpWallet_ZeroAmount_ThrowsValidationException()
    {
        var user = UserService.Register("alice", "alice@example.com", "pass123", UserRole.Customer);

        Assert.Throws<ValidationException>(() => UserService.TopUpWallet(user, 0m));
    }

    [Fact]
    public void TopUpWallet_NegativeAmount_ThrowsValidationException()
    {
        var user = UserService.Register("alice", "alice@example.com", "pass123", UserRole.Customer);

        Assert.Throws<ValidationException>(() => UserService.TopUpWallet(user, -50m));
    }
}
