namespace MiniBitcoin;

public class BitcoinMarket
{
    private readonly Wallet wallet;
    private decimal quote;
    public decimal Quote
    {
        get => quote;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "A cotação deve ser positiva.");
            quote = value;
        }
    }

    public BitcoinMarket(Wallet wallet, decimal quote)
    {
        this.wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        Quote = quote;
    }

    public void Buy(decimal brl)
    {
        if (brl <= 0 || brl > wallet.Brl)
            throw new ArgumentOutOfRangeException(nameof(brl), "Valor inválido ou saldo BRL insuficiente.");
        decimal btc = decimal.Floor(brl / Quote * 100000000m) / 100000000m;
        if (btc <= 0) throw new ArgumentOutOfRangeException(nameof(brl), "O mínimo é um satoshi.");
        decimal cost = btc * Quote;
        wallet.ApplyTrade(wallet.Brl - cost, wallet.Bitcoin + btc);
    }

    public void Sell(decimal btc)
    {
        if (btc <= 0 || btc > wallet.Bitcoin || decimal.Round(btc, 8) != btc)
            throw new ArgumentOutOfRangeException(nameof(btc), "Valor inválido ou saldo BTC insuficiente (máximo de 8 casas decimais).");
        decimal proceeds = btc * Quote;
        wallet.ApplyTrade(wallet.Brl + proceeds, wallet.Bitcoin - btc);
    }
}
