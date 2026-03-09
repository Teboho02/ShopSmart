namespace ShopSmart.Data;

using ShopSmart.Models;

public class VoucherRepository
{
    private static readonly string FilePath =
        Path.Combine(AppContext.BaseDirectory, "data", "vouchers.json");

    private readonly AppData _data;

    public VoucherRepository(AppData data)
    {
        _data = data;

        var loaded = JsonFileStore.Load<Voucher>(FilePath);
        _data.Vouchers.AddRange(loaded);
    }

    public Voucher? FindByCode(string code) =>
        _data.Vouchers.FirstOrDefault(v =>
            v.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase));

    public void Add(Voucher voucher)
    {
        _data.Vouchers.Add(voucher);
        JsonFileStore.Save(FilePath, _data.Vouchers);
    }

    public void Save() => JsonFileStore.Save(FilePath, _data.Vouchers);
}
