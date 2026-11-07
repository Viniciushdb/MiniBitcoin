// See https://aka.ms/new-console-template for more information
using System.Globalization;
using MiniBitcoin;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
var wallet = new Wallet(10000m, 0m);
var market = new BitcoinMarket(wallet, 300000m);
Console.WriteLine("MiniBitcoin — simulação local, sem operações reais.");
Console.WriteLine("Saldo inicial: R$ 10.000,00. Cotação simulada: R$ 300.000,00.");
while (true)
{
    Console.WriteLine("\n1 Cotação | 2 Saldos | 3 Comprar | 4 Vender | 5 Extrato | 6 Atualizar cotação | 0 Sair");
    string? option = Console.ReadLine();
    if (option is null or "0") break;
    try
    {
        switch (option)
        {
            case "1":
                Console.WriteLine($"Cotação simulada: {market.Quote:C2} / BTC");
                break;
            case "2":
                Console.WriteLine($"BRL: {wallet.Brl:C2} | BTC: {wallet.Bitcoin:F8} | Satoshis: {wallet.Bitcoin * 100000000m:0}");
                Console.WriteLine($"Patrimônio: {market.PortfolioValue:C2}");
                break;
            case "3":
                market.Buy(ReadAmount("Valor máximo da compra em BRL (ex.: 150,50): "));
                Console.WriteLine("Compra realizada. Frações menores que um satoshi permanecem no saldo BRL.");
                break;
            case "4":
                market.Sell(ReadAmount("Quantidade de BTC para vender (ex.: 0,001): "));
                Console.WriteLine("Venda realizada.");
                break;
            case "5":
                if (market.History.Count == 0) Console.WriteLine("Nenhuma transação realizada.");
                foreach (var t in market.History)
                    Console.WriteLine($"{t.Timestamp.ToLocalTime():dd/MM/yyyy HH:mm:ss} | {t.Type} | BTC {t.Bitcoin:F8} | {t.Brl:C2} | Cotação {t.Quote:C2}");
                Console.WriteLine($"Patrimônio atual: {market.PortfolioValue:C2}");
                break;
            case "6":
                market.Quote = ReadAmount("Nova cotação simulada em BRL: ");
                Console.WriteLine("Cotação atualizada.");
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (ArgumentException ex) { Console.WriteLine($"Operação recusada: {ex.Message}"); }
    catch (OverflowException) { Console.WriteLine("Valor muito alto. Operação recusada."); }
    catch (EndOfStreamException) { break; }
}

static decimal ReadAmount(string prompt)
{
    Console.Write(prompt);
    var input = Console.ReadLine() ?? throw new EndOfStreamException();
    if (!decimal.TryParse(input, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
        CultureInfo.CurrentCulture, out decimal amount))
        throw new ArgumentException("Digite um número válido, usando vírgula como separador decimal e sem separador de milhar.");
    return amount;
}
