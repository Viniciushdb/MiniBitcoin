namespace MiniBitcoin;

public class Wallet
{
    public decimal Brl { get; private set; }
    public decimal Bitcoin { get; private set; }

    public Wallet(decimal brl, decimal bitcoin)
    {
        if (brl < 0 || bitcoin < 0)
            throw new ArgumentOutOfRangeException(nameof(brl), "Os saldos não podem ser negativos.");
        Brl = brl;
        Bitcoin = bitcoin;
    }

    public void DebitBrl(decimal amount)
    {
        if (amount <= 0 || amount > Brl)
            throw new ArgumentOutOfRangeException(nameof(amount), "Débito inválido ou saldo BRL insuficiente.");
        Brl -= amount;
    }

    public void DebitBitcoin(decimal amount)
    {
        if (amount <= 0 || amount > Bitcoin)
            throw new ArgumentOutOfRangeException(nameof(amount), "Débito inválido ou saldo BTC insuficiente.");
        Bitcoin -= amount;
    }

    internal void ApplyTrade(decimal brl, decimal bitcoin)
    {
        if (brl < 0 || bitcoin < 0)
            throw new ArgumentOutOfRangeException(nameof(brl));
        Brl = brl;
        Bitcoin = bitcoin;
    }
}
