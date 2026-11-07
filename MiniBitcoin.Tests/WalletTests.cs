using MiniBitcoin;

namespace MiniBitcoin.Tests;

public class WalletTests
{
    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    public void RejectsNegativeInitialBalances(decimal brl, decimal btc) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new Wallet(brl, btc));

    [Fact]
    public void PreservesInitialBalances()
    {
        var wallet = new Wallet(100m, 0.5m);
        Assert.Equal(100m, wallet.Brl);
        Assert.Equal(0.5m, wallet.Bitcoin);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void RejectsInvalidBrlDebitWithoutChangingBalance(decimal amount)
    {
        var wallet = new Wallet(100m, 1m);
        Assert.ThrowsAny<ArgumentException>(() => wallet.DebitBrl(amount));
        Assert.Equal(100m, wallet.Brl);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(2)]
    public void RejectsInvalidBitcoinDebitWithoutChangingBalance(decimal amount)
    {
        var wallet = new Wallet(100m, 1m);
        Assert.ThrowsAny<ArgumentException>(() => wallet.DebitBitcoin(amount));
        Assert.Equal(1m, wallet.Bitcoin);
    }

    [Fact]
    public void AllowsDebitingEntireBalances()
    {
        var wallet = new Wallet(100m, 1m);
        wallet.DebitBrl(100m);
        wallet.DebitBitcoin(1m);
        Assert.Equal(0m, wallet.Brl);
        Assert.Equal(0m, wallet.Bitcoin);
    }
}
