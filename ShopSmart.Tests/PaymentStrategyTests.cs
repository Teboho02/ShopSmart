using ShopSmart.Enums;
using ShopSmart.Models;
using ShopSmart.Services;
using ShopSmart.Services.Payments;

public class PaymentStrategyTests : TestBase
{
    // ─── WalletPaymentStrategy ───────────────────────────────────────────────

    [Fact]
    public void Wallet_SufficientBalance_DeductsAndReturnsCompleted()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        user.WalletBalance = 100m;
        var strategy = new WalletPaymentStrategy(UserRepo);

        var status = strategy.Execute(user, 60m);

        Assert.Equal(PaymentStatus.Completed, status);
        Assert.Equal(40m, user.WalletBalance);
    }

    [Fact]
    public void Wallet_ExactBalance_Succeeds()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        user.WalletBalance = 50m;

        var status = new WalletPaymentStrategy(UserRepo).Execute(user, 50m);

        Assert.Equal(PaymentStatus.Completed, status);
        Assert.Equal(0m, user.WalletBalance);
    }

    [Fact]
    public void Wallet_InsufficientBalance_ThrowsValidationException()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        user.WalletBalance = 10m;

        Assert.Throws<ValidationException>(() =>
            new WalletPaymentStrategy(UserRepo).Execute(user, 50m));
    }

    [Fact]
    public void Wallet_MethodName_IsWallet()
    {
        Assert.Equal("Wallet", new WalletPaymentStrategy(UserRepo).MethodName);
    }

    // ─── EftPaymentStrategy ──────────────────────────────────────────────────

    [Fact]
    public void Eft_ReturnsPending_NoWalletDeduction()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        user.WalletBalance = 100m;

        var status = new EftPaymentStrategy().Execute(user, 100m);

        Assert.Equal(PaymentStatus.Pending, status);
        Assert.Equal(100m, user.WalletBalance); // untouched
    }

    [Fact]
    public void Eft_MethodName_IsEft()
    {
        Assert.Equal("EFT", new EftPaymentStrategy().MethodName);
    }

    // ─── PayPalPaymentStrategy ───────────────────────────────────────────────

    [Fact]
    public void PayPal_ValidEmail_ReturnsCompleted()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);

        var status = new PayPalPaymentStrategy("alice@paypal.com").Execute(user, 50m);

        Assert.Equal(PaymentStatus.Completed, status);
    }

    [Fact]
    public void PayPal_InvalidEmail_ThrowsValidationException()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);

        Assert.Throws<ValidationException>(() =>
            new PayPalPaymentStrategy("notanemail").Execute(user, 50m));
    }

    [Fact]
    public void PayPal_MethodName_IncludesEmail()
    {
        var strategy = new PayPalPaymentStrategy("alice@paypal.com");

        Assert.Contains("alice@paypal.com", strategy.MethodName);
    }

    // ─── VoucherPaymentStrategy ──────────────────────────────────────────────

    [Fact]
    public void Voucher_FullCoverage_WalletUntouched()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        user.WalletBalance = 200m;
        VoucherRepo.Add(new Voucher("SAVE50", 50m));

        new VoucherPaymentStrategy("SAVE50", VoucherRepo, UserRepo).Execute(user, 30m);

        Assert.Equal(200m, user.WalletBalance);
    }

    [Fact]
    public void Voucher_FullCoverage_MarksVoucherRedeemed()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        VoucherRepo.Add(new Voucher("SAVE50", 50m));

        new VoucherPaymentStrategy("SAVE50", VoucherRepo, UserRepo).Execute(user, 30m);

        Assert.True(VoucherRepo.FindByCode("SAVE50")!.IsRedeemed);
    }

    [Fact]
    public void Voucher_FullCoverage_ReturnsCompleted()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        VoucherRepo.Add(new Voucher("SAVE50", 50m));

        var status = new VoucherPaymentStrategy("SAVE50", VoucherRepo, UserRepo).Execute(user, 30m);

        Assert.Equal(PaymentStatus.Completed, status);
    }

    [Fact]
    public void Voucher_PartialCoverage_ChargesRemainderToWallet()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        user.WalletBalance = 100m;
        VoucherRepo.Add(new Voucher("SAVE50", 50m));

        // Total = 80, voucher = 50, remainder = 30
        new VoucherPaymentStrategy("SAVE50", VoucherRepo, UserRepo).Execute(user, 80m);

        Assert.Equal(70m, user.WalletBalance); // 100 - 30
    }

    [Fact]
    public void Voucher_PartialCoverage_InsufficientWallet_ThrowsValidationException()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        user.WalletBalance = 5m;
        VoucherRepo.Add(new Voucher("SAVE10", 10m));

        // Total = 50, voucher = 10, remainder = 40, wallet = 5 → fail
        Assert.Throws<ValidationException>(() =>
            new VoucherPaymentStrategy("SAVE10", VoucherRepo, UserRepo).Execute(user, 50m));
    }

    [Fact]
    public void Voucher_InvalidCode_ThrowsValidationException()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);

        Assert.Throws<ValidationException>(() =>
            new VoucherPaymentStrategy("BADCODE", VoucherRepo, UserRepo).Execute(user, 50m));
    }

    [Fact]
    public void Voucher_AlreadyRedeemed_ThrowsValidationException()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        VoucherRepo.Add(new Voucher("SAVE50", 50m, isRedeemed: true));

        Assert.Throws<ValidationException>(() =>
            new VoucherPaymentStrategy("SAVE50", VoucherRepo, UserRepo).Execute(user, 30m));
    }

    [Fact]
    public void Voucher_CodeLookupIsCaseInsensitive()
    {
        var user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        VoucherRepo.Add(new Voucher("SAVE50", 50m));

        // lowercase code
        var status = new VoucherPaymentStrategy("save50", VoucherRepo, UserRepo).Execute(user, 30m);

        Assert.Equal(PaymentStatus.Completed, status);
    }
}
