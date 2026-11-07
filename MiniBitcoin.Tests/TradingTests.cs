using MiniBitcoin;

namespace MiniBitcoin.Tests;

public class TradingTests
{
    [Fact]
    public void BuysAndSellsAtCurrentQuote()
    {
        var wallet = new Wallet(1000m, 0m);
        var market = new BitcoinMarket(wallet, 100000m);
        market.Buy(500m);
        Assert.Equal(500m, wallet.Brl);
        Assert.Equal(0.005m, wallet.Bitcoin);
        market.Quote = 200000m;
        market.Sell(0.005m);
        Assert.Equal(1500m, wallet.Brl);
        Assert.Equal(0m, wallet.Bitcoin);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectsInvalidQuotes(decimal quote)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BitcoinMarket(new Wallet(0, 0), quote));
        var market = new BitcoinMarket(new Wallet(0, 0), 100m);
        Assert.Throws<ArgumentOutOfRangeException>(() => market.Quote = quote);
        Assert.Equal(100m, market.Quote);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1001)]
    public void RejectsInvalidPurchases(decimal amount)
    {
        var wallet = new Wallet(1000m, 1m);
        var market = new BitcoinMarket(wallet, 100000m);
        Assert.ThrowsAny<ArgumentException>(() => market.Buy(amount));
        Assert.Equal(1000m, wallet.Brl);
        Assert.Equal(1m, wallet.Bitcoin);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(2)]
    public void RejectsInvalidSales(decimal amount)
    {
        var wallet = new Wallet(1000m, 1m);
        var market = new BitcoinMarket(wallet, 100000m);
        Assert.ThrowsAny<ArgumentException>(() => market.Sell(amount));
        Assert.Equal(1000m, wallet.Brl);
        Assert.Equal(1m, wallet.Bitcoin);
    }

    [Fact]
    public void RejectsPurchaseSmallerThanOneSatoshi()
    {
        var market = new BitcoinMarket(new Wallet(1000, 0), 100000m);
        Assert.Throws<ArgumentOutOfRangeException>(() => market.Buy(0.0001m));
    }
}
