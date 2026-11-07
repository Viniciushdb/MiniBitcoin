using MiniBitcoin;

namespace MiniBitcoin.Tests;

public class LedgerTests
{
    [Fact]
    public void RecordsSuccessfulTradesInOrderAndValuesPortfolioAtCurrentQuote()
    {
        var market = new BitcoinMarket(new Wallet(1000m, 0m), 100000m);
        Assert.Empty(market.History);
        market.Buy(500m);
        market.Quote = 200000m;
        Assert.Equal(1500m, market.PortfolioValue);
        market.Sell(0.001m);
        Assert.Collection(market.History,
            t => { Assert.Equal("Compra", t.Type); Assert.Equal(500m, t.Brl); Assert.Equal(0.005m, t.Bitcoin); Assert.Equal(100000m, t.Quote); Assert.NotEqual(default, t.Timestamp); },
            t => { Assert.Equal("Venda", t.Type); Assert.Equal(200m, t.Brl); Assert.Equal(0.001m, t.Bitcoin); Assert.Equal(200000m, t.Quote); });
        Assert.Equal(1500m, market.PortfolioValue);
    }

    [Fact]
    public void FailedTradesDoNotCreateHistory()
    {
        var market = new BitcoinMarket(new Wallet(0, 0), 100000);
        Assert.Throws<ArgumentOutOfRangeException>(() => market.Buy(1));
        Assert.Throws<ArgumentOutOfRangeException>(() => market.Sell(1));
        Assert.Empty(market.History);
        Assert.Equal(0m, market.PortfolioValue);
    }
}
