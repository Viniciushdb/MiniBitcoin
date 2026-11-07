namespace MiniBitcoin;

public record Transaction(string Type, decimal Bitcoin, decimal Brl, decimal Quote, DateTimeOffset Timestamp);
