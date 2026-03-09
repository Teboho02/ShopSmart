namespace ShopSmart.Models;

using System.Text.Json.Serialization;

public class Voucher
{
    public string  Code       { get; init; }
    public decimal Value      { get; init; }
    public bool    IsRedeemed { get; set; }

    [JsonConstructor]
    public Voucher(string code, decimal value, bool isRedeemed)
    {
        Code       = code;
        Value      = value;
        IsRedeemed = isRedeemed;
    }

    public Voucher(string code, decimal value)
        : this(code, value, isRedeemed: false) { }
}
